using Newtonsoft.Json;
using O1shows.Models;
using O1shows.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace O1shows.Services
{
    public interface IProfileService
    {
        Task<UserProfileViewModel> GetUserProfileAsync(int id);
        Task<float> SelectSeriesRaiting(int UserRaiting, int SeriesId);
        Task SelectWatchStatus(int SeriesId, string StatusName);
        Task CheckEpisodes(int[] CheckedIds, int SeriesId);
        Task<Comment> AddComment(string CommentText, int EpisodeId);
        Task AddFriend(int FriendId);
        Task RemoveFriend(int FriendId);
        Task<FriendsViewModel> GetFriends();
        Task<SeriesCatalogViewModel> GetRecommendations(int ProfileId);
    }
    public class ProfileService : IProfileService
    {
        public IApiRequestService ApiService => DependencyService.Get<IApiRequestService>();
        public async Task<UserProfileViewModel> GetUserProfileAsync(int id)
        {
            string ControllerName = "ProfileAPI";
            string ActionName = "GetUserProfile";
            var parameter = new { id };
            var response = await ApiService.GetAsync(ControllerName, ActionName, parameter);
            if (response != "Failed")
            {
                UserProfileViewModel model = JsonConvert.DeserializeObject<UserProfileViewModel>(response);
                double width = (Application.Current.MainPage.Width - 20) / 2;
                foreach (var tab in model.StatusTabs)
                {
                    foreach (var series in tab.SeriesList)
                    {
                        series.PicturePath = "http://192.168.0.9/" + series.PicturePath.Replace("normal", "small");
                        series.SeriesBlockWidth = width;
                        series.Progress = (double)series.UserEpisodes.Count / series.EpisodesCount;
                    }
                }
                return model;
            }
            return default;
        }
        public async Task<float> SelectSeriesRaiting(int UserRaiting, int SeriesId)
        {
            string ControllerName = "ProfileAPI";
            string ActionName = "SelectSeriesRaiting";
            var parameters = new 
            { 
                UserRaiting = UserRaiting,
                SeriesId = SeriesId
            };
            string response = await ApiService.PostAsync(ControllerName, ActionName, parameters);
            if (response != "Failed")
            {
                return float.Parse(response, CultureInfo.InvariantCulture.NumberFormat);
            }
            return default;
        }
        public async Task SelectWatchStatus(int SeriesId, string StatusName)
        {
            string ControllerName = "ProfileAPI";
            string ActionName = "SelectWatchStatus";
            var parameters = new
            {
                SeriesId = SeriesId,
                StatusName = StatusName
            };
            await ApiService.PostAsync(ControllerName, ActionName, parameters);
        }
        public async Task CheckEpisodes(int[] CheckedIds, int SeriesId)
        {
            string ControllerName = "ProfileAPI";
            string ActionName = "CheckEpisodes";
            var parameters = new
            {
                CheckedIds = CheckedIds,
                SeriesId = SeriesId
            };
            await ApiService.PostAsync(ControllerName, ActionName, parameters);
        }
        public async Task<Comment> AddComment(string CommentText, int EpisodeId)
        {
            string ControllerName = "ProfileAPI";
            string ActionName = "AddComment";
            var parameters = new
            {
                CommentText = CommentText,
                EpisodeId = EpisodeId
            };
            var response = await ApiService.PostAsync(ControllerName, ActionName, parameters);
            if (response != "Failed")
            {
                return JsonConvert.DeserializeObject<Comment>(response);
            }
            return default;
        }
        public async Task AddFriend(int FriendId)
        {
            string ControllerName = "ProfileAPI";
            string ActionName = "AddFriend";
            var parameters = new
            {
                FriendId = FriendId,
            };
            await ApiService.GetAsync(ControllerName, ActionName, parameters);
        }
        public async Task RemoveFriend(int FriendId)
        {
            string ControllerName = "ProfileAPI";
            string ActionName = "RemoveFriend";
            var parameters = new
            {
                FriendId = FriendId,
            };
            await ApiService.GetAsync(ControllerName, ActionName, parameters);
        }
        public async Task<FriendsViewModel> GetFriends()
        {
            string ControllerName = "ProfileAPI";
            string ActionName = "GetFriends";
            var parameters = new {};
            var response = await ApiService.GetAsync(ControllerName, ActionName, parameters);
            if (response != "Failed")
            {
                FriendsViewModel model = JsonConvert.DeserializeObject<FriendsViewModel>(response);
                foreach (UserProfile profile in model.Friends)
                {
                    profile.ImageSrc = "http://192.168.0.9/" + profile.ImageSrc;
                }
                return model;
            }
            return default;
        }
        public async Task<SeriesCatalogViewModel> GetRecommendations(int ProfileId)
        {
            string ControllerName = "ProfileAPI";
            string ActionName = "GetRecommendations";
            var parameters = new
            {
                ProfileId = ProfileId
            };
            var response = await ApiService.GetAsync(ControllerName, ActionName, parameters);
            if (response != "Failed")
            {
                SeriesCatalogViewModel model = JsonConvert.DeserializeObject<SeriesCatalogViewModel>(response);
                double width = (Application.Current.MainPage.Width - 20) / 2;
                foreach (var series in model.SeriesListView)
                {
                    series.PicturePath = "http://192.168.0.9/" + series.PicturePath.Replace("normal", "small");
                    series.SeriesBlockWidth = width;
                    series.SeriesRaiting = new SeriesRaiting(series.Raiting, "01Shows");
                }
                return model;
            }
            return default;
        }
    }
}
