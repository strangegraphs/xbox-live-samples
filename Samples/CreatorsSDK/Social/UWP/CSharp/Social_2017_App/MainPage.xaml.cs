using Microsoft.Xbox.Services;
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
using Windows.System;
using Windows.UI.Core;
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
        CoreDispatcher UIDispatcher = null;

        public MainPage()
        {
            this.InitializeComponent();
            UIDispatcher = Window.Current.CoreWindow.Dispatcher;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            XboxLiveUwpImplementation xboxLiveImplementation = new XboxLiveUwpImplementation();
            var systemUsers = await xboxLiveImplementation.GetAllSystemUsers();
            var xboxUsers = await xboxLiveImplementation.ConvertSystemUsersToXboxUsers(systemUsers, UIDispatcher);
            var xboxContexts = xboxLiveImplementation.ConvertXboxUserListToXboxContextList(xboxUsers);
        }
    }
}
