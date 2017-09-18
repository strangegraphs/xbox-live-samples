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
using Microsoft.Xbox.Services.Statistics.Manager;
using Microsoft.Xbox.Services.Leaderboard;
using Microsoft.Xbox.Services.System;

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

        public async Task<bool> SignInXboxUser(Page page, XboxLiveContext xboxLiveContext)
        {
            SignInResult signInResult = await xboxLiveContext.User.SignInAsync(page);
            return true;
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
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<XboxSocialRelationship>> GetSocialRelationshipsAsync(
            Page page,
            XboxLiveContext xboxLiveContext,
            uint startIndex = 0,
            uint maxFriendsToRetrieve = 10,
            string serviceConfigurationId = "12200100-88da-4d8b-af88-e38f5d2a2bca",
            string presenceId = "rpdemo")
        {
            PresenceData presenceData = new PresenceData(serviceConfigurationId, presenceId);
            await xboxLiveContext.PresenceService.SetPresenceAsync(true, presenceData);

            XboxSocialRelationshipResult relationshipResult = await xboxLiveContext.SocialService.GetSocialRelationshipsAsync(
                SocialRelationship.All,
                startIndex,
                maxFriendsToRetrieve
            );

            List<XboxSocialRelationship> listOfRelationships = new List<XboxSocialRelationship>();
            foreach (var xboxRelationship in relationshipResult.Items)
            {
                listOfRelationships.Add(xboxRelationship);
            }

            return listOfRelationships;
        }

        IReadOnlyList<string> GetStat(Page page, XboxLiveContext xboxLiveContext)
        {
            StatisticManager statisticManager = StatisticManager.SingletonInstance;
            if (statisticManager == null)
            {
                return null;
            }
            return statisticManager.GetStatisticNames(xboxLiveContext.User);
        }

        public bool WriteStat(Page page, XboxLiveContext xboxLiveContext, string statNameToChange = "HighScore", long statValue = 1002)
        {
            StatisticManager statisticManager = StatisticManager.SingletonInstance;
            if (statisticManager == null)
            {
                return false;
            }

            statisticManager.SetStatisticIntegerData(xboxLiveContext.User, statNameToChange, statValue);
            return true;
        }

        public bool DeleteStat(Page page, XboxLiveContext xboxLiveContext, string statNameToDelete = "HighScore")
        {
            StatisticManager statisticManager = StatisticManager.SingletonInstance;
            if (statisticManager == null)
            {
                return false;
            }

            statisticManager.DeleteStatistic(xboxLiveContext.User, statNameToDelete);
            return true;
        }

        public LeaderboardQuery GetLeaderboards(Page page, XboxLiveContext xboxLiveContext, string statNameLeaderBoardIsBasedOn)
        {
            StatisticManager statisticManager = StatisticManager.SingletonInstance;
            if (statisticManager == null)
            {
                return null;
            }

            LeaderboardQuery query = new LeaderboardQuery();

            var statValue = statisticManager.GetStatistic(xboxLiveContext.User, statNameLeaderBoardIsBasedOn);
            string name = statValue.Name;
            var intValue = statValue.AsInteger;

            statisticManager.GetLeaderboard(xboxLiveContext.User, statNameLeaderBoardIsBasedOn, query);

            return query;
        }
    }
}
