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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ASM.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        Member currentMemberLogin;
        Token currentToken;
        public Login()
        {
            currentToken = new Token();
            currentMemberLogin = new Member();
            check();
            this.InitializeComponent();
           
        }
        #region Void Check

        public async void check()
        {
            try {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("Token.txt");
                currentToken.token = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                if (currentToken.token != null)
                {
                    logout.Visibility = Visibility.Visible;
                    login.Visibility = Visibility.Collapsed;
                }
            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Event Login
        private async void Do_login(object sender, RoutedEventArgs e)
        {
            this.currentMemberLogin.email = this.lEmail.Text;
            this.currentMemberLogin.password = this.lPassword.Password;

            string jsonMember = JsonConvert.SerializeObject(this.currentMemberLogin);
            Debug.WriteLine(jsonMember);

            HttpClient httpClient = new HttpClient();
            var content = new StringContent(jsonMember, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(Service.ServiceURL.MEMBER_LOGIN, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(contents);

            currentToken = JsonConvert.DeserializeObject<Token>(contents);
            Debug.WriteLine(currentToken.token);
            //this.lEmail.Text = string.Empty;
            //this.lPassword.Password = string.Empty;

            if (response.Result.StatusCode == HttpStatusCode.Created)
            {
                await CreatedFileToken();
                
                login.Visibility = Visibility.Collapsed;
                logout.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(contents);

                this.ValidateError.Text = "* " +  errorResponse.message;
                this.ValidateError.Visibility = Visibility.Collapsed;
              
            }
        }

        #endregion
        //private async void ReadFileToken()
        //{
        //    Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        //    Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("Token.txt");
        //    currentToken.token = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
        //}


        #region Void CreatedFileToken()
        private async Task CreatedFileToken()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("Token.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            sampleFile = await storageFolder.GetFileAsync("Token.txt");
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile,currentToken.token);
        
        }
        #endregion

        #region Void DeleteFileToken()
        private async void DeleteFileToken()
        {
            StorageFile filed = await ApplicationData.Current.LocalFolder.GetFileAsync("Token.txt");
            if (filed != null)
            {
                await filed.DeleteAsync();

            }
        }
        #endregion

        private void Go_Rigister(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.Register));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteFileToken();
            logout.Visibility = Visibility.Collapsed;
            login.Visibility = Visibility.Visible;
        }

        private void Go_LoadInfor(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.UserInfo));
        }

        private void Go_UploadMusic(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.CreateSong));
        }

        private void Go_Music(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.CreateSong));
        }
    }
}
