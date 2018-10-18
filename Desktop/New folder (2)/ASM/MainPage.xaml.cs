using System;
using System.Collections.Generic;
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
        public MainPage()
        {
            this.InitializeComponent();
            
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
                        break;
                    case "Contact_Info":
                        ContentFrane.Navigate(typeof(Views.UserInfo));
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }
}
