using ASM.Emtity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ASM.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        private StorageFile photo;
        private string CurrenUploadURL;
        Member _currentMember;
        public Register()
        {
            _currentMember = new Member();
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }


        /// <summary>
        /// Chụp ảnh
        /// Capture your photos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Capture(object sender, RoutedEventArgs e)
        {

            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                // User cancelled photo capture
                return;
            }

            HttpClient httpClient = new HttpClient();
            CurrenUploadURL = await httpClient.GetStringAsync(Service.ServiceURL.GET_UPLOAD_URL);
            Debug.WriteLine("Upload url: " + CurrenUploadURL);
            HttpUploadFile(CurrenUploadURL, "myFile", "image/png");
        }

        /// <summary>
        /// Upload Image
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <param name="contentType"></param>
        public async void HttpUploadFile(string url, string paramName, string contentType)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";

            Stream rs = await wr.GetRequestStreamAsync();
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, "path_file", contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            // write file.
            Stream fileStream = await this.photo.OpenStreamForReadAsync();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);

            WebResponse wresp = null;
            try
            {
                wresp = await wr.GetResponseAsync();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //Debug.WriteLine(string.Format("File uploaded, server response is: @{0}@", reader2.ReadToEnd()));
                //string imgUrl = reader2.ReadToEnd();
                //Uri u = new Uri(reader2.ReadToEnd(), UriKind.Absolute);
                //Debug.WriteLine(u.AbsoluteUri);
                //ImageUrl.Text = u.AbsoluteUri;
                //MyAvatar.Source = new BitmapImage(u);
                //Debug.WriteLine(reader2.ReadToEnd());
                string imageUrl = reader2.ReadToEnd();
                rImage.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                rAvatar.Text = imageUrl;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error uploading file", ex.StackTrace);
                Debug.WriteLine("Error uploading file", ex.InnerException);
                if (wresp != null)
                {
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

        /// <summary>
        /// Chọn Ngày/Tháng/Năm sinh
        /// Choose date of birth
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Do_Dateirthday(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            this._currentMember.birthday = rBirthday.Date.Value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Chọn giới tính
        /// Choose Gender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Do_CheckGender(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            this._currentMember.gender = Int32.Parse(radio.Tag.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Do_Register(object sender, RoutedEventArgs e)
        {
            this._currentMember.firstName = this.rFirstName.Text;
            this._currentMember.lastName = this.rLastName.Text;
            this._currentMember.email = this.rEmail.Text;
            this._currentMember.password = this.rPassword.Password;
            this._currentMember.phone = this.rPhone.Text;
            this._currentMember.avatar = this.rAvatar.Text;
            this._currentMember.address = this.rAddress.Text;
            this._currentMember.introduction = this.rIntro.Text;


            // ép kiểu từ string sang Json
            string JsonMember = JsonConvert.SerializeObject(this._currentMember);
            Debug.WriteLine(JsonMember);
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonMember, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(Service.ServiceURL.MEMBER_REGISTER, content);
            this.rFirstName.Text = string.Empty;
            this.rLastName.Text = string.Empty;
            this.rEmail.Text = string.Empty;
            this.rPassword.Password = string.Empty;
            this.rAvatar.Text = string.Empty;
            this.rPhone.Text = string.Empty;
            this.rAddress.Text = string.Empty;
            this.rIntro.Text = string.Empty;
        }

        private void Do_Reset(object sender, RoutedEventArgs e)
        {
            this.rFirstName.Text = string.Empty;
            this.rLastName.Text = string.Empty;
            this.rEmail.Text = string.Empty;
            this.rPassword.Password = string.Empty;
            this.rAvatar.Text = string.Empty;
            this.rPhone.Text = string.Empty;
            this.rAddress.Text = string.Empty;
            this.rIntro.Text = string.Empty;

        }
    }
}
