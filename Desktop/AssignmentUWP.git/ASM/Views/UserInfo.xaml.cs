using ASM.Emtity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class UserInfo : Page
    {
        Token currenttoken;
        public UserInfo()
        {
            currenttoken = new Token();
            this.InitializeComponent();
            LoadInformation();
        }

        private async void LoadInformation()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("Token.txt");
                currenttoken.token = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                
                HttpClient httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + currenttoken.token);
                Debug.WriteLine(currenttoken.token);
                string reponese = await httpClient.GetStringAsync(Service.ServiceURL.MEMBER_INFORMATION);

                Member member = JsonConvert.DeserializeObject<Member>(reponese);
                Uri url = new Uri(member.avatar, UriKind.Absolute);

                BitmapImage bmImage = new BitmapImage(url);
                this.infoAvatar.Source = bmImage;
                Debug.WriteLine(reponese);

                this.Ulable.Text = "User Name";
                this.UserNameLable.Text = ": " + member.firstName + member.lastName;
                this.Elable.Text = "Email";
                this.EmailLable.Text = ": " + member.email;
                this.Plable.Text = "PassWord";
                this.PassLable.Text = ": " + member.password;
                this.Phlable.Text = "Phone";
                this.Glable.Text = "Gender";
                switch (member.gender.ToString())
                {
                    case "0":
                        this.GenderLable.Text = ": Other";
                        break;
                    case "1":
                        this.GenderLable.Text = ": Male";
                        break;
                    case "2":
                        this.GenderLable.Text = ": Female";
                        break;
                    default:
                        break;
                }
                this.PhoneLable.Text = ": " + member.phone;
                this.Dlable.Text = "Date";
                this.DateLable.Text = ": " + member.birthday;
                this.Alable.Text = "Address";
                this.AddressLable.Text = ": " + member.address;
                this.ILable.Text = "Informations";
                this.IntroLable.Text = ": " + member.introduction;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
