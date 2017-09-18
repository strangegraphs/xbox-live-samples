using Microsoft.Xbox.Services.System;
using Social_2017.XboxLiveUwpImplementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace Social_2017_App
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

        public async Task<bool> SignInUser()
        {
            try
            {
                XboxLiveUwpImplementation xboxSocialObject = new XboxLiveUwpImplementation();
                XboxLiveUser xboxLiveUser = new XboxLiveUser();
                var xboxLiveContext = new Microsoft.Xbox.Services.XboxLiveContext(xboxLiveUser);
                return await xboxSocialObject.SignInXboxUser(this, xboxLiveContext);
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SignInUser();
        }
    }
}
