using ASM.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ASM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string currentToken;
        public MainPage()
        {
            
            this.InitializeComponent();

        }
        private async void abcd()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("Token.txt");
            currentToken = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
        }


        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

            if (args.IsSettingsSelected)
            {
                ContentFrane.Navigate(typeof(Views.Login));
            }
            else
            {
                NavigationViewItem items = args.SelectedItem as NavigationViewItem;

                switch (items.Tag.ToString())
                {
                    case "sign_up":
                        ContentFrane.Navigate(typeof(Views.Register));
                        break;
                    case "sign_in":
                        ContentFrane.Navigate(typeof(Views.Login));
                        //try
                        //{
                        //    do
                        //    {
                        //        abcd();
                        //    }
                        //    while (currentToken != null);
                        //    {
                        //        items.Visibility = Visibility.Collapsed;
                        //    }
                        //}catch(Exception ex)
                        //{
                        //    Debug.WriteLine(ex.Message);
                        //}
                        //items.Visibility = Visibility.Collapsed;
                        break;
                    case "Contact_Info":
                        ContentFrane.Navigate(typeof(Views.UserInfo));
                        break;
                    case "Music":
                        items.Visibility = Visibility.Visible;
                        ContentFrane.Navigate(typeof(Views.Login));
                        break;
                    default:
                        break;
                }
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
