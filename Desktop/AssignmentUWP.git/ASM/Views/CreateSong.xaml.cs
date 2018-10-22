using ASM.Emtity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class CreateSong : Page
    {
        Song _currentSong;
        private string Token;
        public CreateSong()
        {
            _currentSong = new Song();
            this.InitializeComponent();
        }

        private async void Create_Song(object sender, RoutedEventArgs e)
        {
            _currentSong.name = NameSong.Text;
            _currentSong.description = Description.Text;
            _currentSong.singer = Singer.Text;
            _currentSong.author = Author.Text;
            _currentSong.thumbnail = Thumbnail.Text;
            _currentSong.link = Link.Text;

            HttpClient httpclient = new HttpClient();
            try
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("Token.txt");
               Token = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                Debug.Write(Token);
                if (Token != null)
                {
                    httpclient.DefaultRequestHeaders.Add("Authorization", "Basic " + Token);
                    string JsonSong = JsonConvert.SerializeObject(_currentSong);
                    var content = new StringContent(JsonSong, Encoding.UTF8, "application/json");
                    var response = httpclient.PostAsync(Service.ServiceURL.MEMBER_LOGIN, content);
                    var contents = await response.Result.Content.ReadAsStringAsync();
                    Debug.Write(contents);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
