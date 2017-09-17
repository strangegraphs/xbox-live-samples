using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.Presence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;
using System.Collections.Concurrent;
using Microsoft.Xbox.Services.Social;

namespace Social_2017.XboxLiveUwpImplementations
{
    public class XboxLiveUwpImplementation
    {
        public EventRegistrationToken m_devicePresenceChangeEventToken = new EventRegistrationToken();
        public EventRegistrationToken m_titlePresenceChangeEventToken = new EventRegistrationToken();

        public void RealTimeActivity_WhenActive(Page page, XboxLiveContext xboxLiveContext)
        {
            xboxLiveContext.RealTimeActivityService.Activate();

            xboxLiveContext.PresenceService.SubscribeToDevicePresenceChange(xboxLiveContext.User.XboxUserId);
            xboxLiveContext.PresenceService.SubscribeToTitlePresenceChange(xboxLiveContext.User.XboxUserId, 0x5D2A2BCA);

            EventHandler<DevicePresenceChangeEventArgs> devicePresenceChangedEvent = (object pageEventCameFrom, DevicePresenceChangeEventArgs eventArgs) => {
                OnDevicePresenceChange((Page)pageEventCameFrom, eventArgs);
            };
            xboxLiveContext.PresenceService.DevicePresenceChanged += devicePresenceChangedEvent;

            EventHandler<TitlePresenceChangeEventArgs> titlePresenceChangedEvent = (object pageEventCameFrom, TitlePresenceChangeEventArgs eventArgs) => {
                OnTitlePresenceChange((Page)pageEventCameFrom, eventArgs);
            };
            xboxLiveContext.PresenceService.TitlePresenceChanged += titlePresenceChangedEvent;
        }

        public static void OnDevicePresenceChange(Page page, DevicePresenceChangeEventArgs args)
        {
            //TODO:: Put logs back in here
        }

        public static void OnTitlePresenceChange(Page page, TitlePresenceChangeEventArgs args)
        {
            //TODO:: Implement This
        }

        public async Task<XboxUserProfile> GetUserProfileAsync(Page page, XboxLiveContext xboxLiveContext)
        {
            try
            {
                TimeSpan giveUpDuration = new TimeSpan(0, 0, 10);
                xboxLiveContext.Settings.HttpRetryDelay = giveUpDuration;
                xboxLiveContext.Settings.EnableServiceCallRoutedEvents = true;

                return await xboxLiveContext.ProfileService.GetUserProfileAsync(xboxLiveContext.User.XboxUserId);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
