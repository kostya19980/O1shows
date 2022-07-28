using NotMyShows.ViewModel.UserProfileService;
using O1shows.Models;
using O1shows.Services;
using O1shows.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace O1shows.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        public IProfileService ProfileService => DependencyService.Get<IProfileService>();
        public ISeriesService SeriesService => DependencyService.Get<ISeriesService>();
        public int Id { get; set; }
        // UserName
        public string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
        // ImageSrc
        public string _imageSrc;
        public string ImageSrc
        {
            get { return _imageSrc; }
            set { SetProperty(ref _imageSrc, value); }
        }
        public bool IsProfileOwner { get; set; }
        public bool IsFriend { get; set; }
        // ProfileStats
        public ProfileStats _profileStats;
        public ProfileStats ProfileStats
        {
            get { return _profileStats; }
            set { SetProperty(ref _profileStats, value); }
        }
        // StatusTabs
        public ObservableCollection<WatchStatusTab> _statusTabs;
        public ObservableCollection<WatchStatusTab> StatusTabs
        {
            get { return _statusTabs; }
            set { SetProperty(ref _statusTabs, value); }
        }
        public IEnumerable<EventGroup> GroupedUserEvents { get; set; }
        // CurrentStatusTab
        public WatchStatusTab _currentStatusTab;
        public WatchStatusTab CurrentStatusTab
        {
            get { return _currentStatusTab; }
            set { SetProperty(ref _currentStatusTab, value); }
        }
        public FriendButton AddFriendButton { get; set; }
        // Commands
        public Command GetUserProfileCommand { get; }
        public Command ToogleStatusTab { get; }
        public Command<int> SelectSeriesCommand { get; }
        public Command<int> FriendButtonCommand { get; }
        public UserProfileViewModel()
        {
            GetUserProfileCommand = new Command(async () => await ExecuteGetUserProfile());
            ToogleStatusTab = new Command<WatchStatusTab>(ExecuteToogleStatusTab);
            SelectSeriesCommand = new Command<int>(ExecuteSelectSeries);
            FriendButtonCommand = new Command<int>(ExecuteFriendButton);
        }
        public async void ExecuteFriendButton(int ProfileId)
        {
            if (IsFriend)
            {
                await ProfileService.RemoveFriend(ProfileId);
                AddFriendButton.BackgroundColor = "#5537EE";
                AddFriendButton.Text = "В друзья";
                AddFriendButton.Icon = "plus.xml";
                IsFriend = false;
            }
            else
            {
                await ProfileService.AddFriend(ProfileId);
                AddFriendButton.BackgroundColor = "#444447";
                AddFriendButton.Text = "В друзьях";
                AddFriendButton.Icon = "x.xml";
                IsFriend = true;
            }
        }
        public async void ExecuteSelectSeries(int seriesId)
        {
            SeriesViewModel model = await SeriesService.GetSeriesAsync(seriesId);
            await Shell.Current.CurrentPage.Navigation.PushModalAsync(new SeriesPage(model));
        }
        public async Task ExecuteGetUserProfile()
        {
            IsBusy = true;

            try
            {
                if(Id == 0)
                {
                    Id = Convert.ToInt32(SecureStorage.GetAsync("userProfileId").Result);
                }
                UserProfileViewModel model = await ProfileService.GetUserProfileAsync(Id);
                Id = model.Id;
                UserName = model.UserName;
                ImageSrc = "http://192.168.0.9/" + model.ImageSrc;
                IsProfileOwner = model.IsProfileOwner;
                IsFriend = model.IsFriend;
                ProfileStats = model.ProfileStats;
                StatusTabs = model.StatusTabs;
                GroupedUserEvents = model.GroupedUserEvents;
                if (!IsProfileOwner)
                {
                    AddFriendButton = new FriendButton()
                    {
                        IsVisible = true
                    };
                    if (IsFriend)
                    {
                        AddFriendButton.BackgroundColor = "#444447";
                        AddFriendButton.Text = "В друзьях";
                        AddFriendButton.Icon = "x.xml";
                    }
                    else
                    {
                        AddFriendButton.BackgroundColor = "#5537EE";
                        AddFriendButton.Text = "В друзья";
                        AddFriendButton.Icon = "plus.xml";
                    }
                }
                else
                {
                    AddFriendButton = new FriendButton()
                    {
                        IsVisible = false
                    };
                }
                if (CurrentStatusTab == null)
                {
                    ExecuteToogleStatusTab(model.StatusTabs[0]);
                }
                else
                {

                    WatchStatusTab current = model.StatusTabs.FirstOrDefault(x => x.WatchStatus.StatusName == CurrentStatusTab.WatchStatus.StatusName);
                    ExecuteToogleStatusTab(current);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void ExecuteToogleStatusTab(WatchStatusTab tab)
        {
            if (CurrentStatusTab != tab)
            {
                Color activeLineColor = (Color)Application.Current.Resources["color-accent-purple"];
                Color activeTextColor = (Color)Application.Current.Resources["color-text-heading"];
                tab.LineColor = activeLineColor;
                tab.TextColor = activeTextColor;
                if (CurrentStatusTab != null)
                {
                    Color inactiveTextColor = (Color)Application.Current.Resources["color-text-secondary"];
                    CurrentStatusTab.TextColor = inactiveTextColor;
                    CurrentStatusTab.LineColor = Color.Transparent;
                }
                tab.LoadedSeriesList = new ObservableCollection<ProfileSeriesItem>();
                foreach (var item in tab.SeriesList.Take(6))
                {
                    tab.LoadedSeriesList.Add(item);
                }
                CurrentStatusTab = tab;
            }
        }
        public void OnAppearing()
        {
            IsBusy = UserName == null;
        }
    }
    public class FriendButton: BaseViewModel
    {
        public string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
        public string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }
        public string _icon;
        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }
        public bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }
    }
    public class ProfileSeriesItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public int EpisodesCount { get; set; }
        public int EpisodeTime { get; set; }
        public SeriesStatus Status { get; set; }
        public string PicturePath { get; set; }
        public int UserRaiting { get; set; }
        public int WatchStatusId { get; set; }
        public DateTime StatusChangedDate { get; set; }
        public DateTime RaitingDate { get; set; }
        public List<UserEpisodeData> UserEpisodes { get; set; }
        public double SeriesBlockWidth { get; set; }
        public double Progress { get; set; }
    }
    public class WatchStatusTab : BaseViewModel
    {
        public WatchStatus WatchStatus { get; set; }
        public List<ProfileSeriesItem> SeriesList { get; set; }
        public ObservableCollection<ProfileSeriesItem> _loadedSeriesList;
        public ObservableCollection<ProfileSeriesItem> LoadedSeriesList
        {
            get { return _loadedSeriesList; }
            set { SetProperty(ref _loadedSeriesList, value); }
        }
        public Color _lineColor;
        public Color LineColor
        {
            get { return _lineColor; }
            set { SetProperty(ref _lineColor, value); }
        }
        public Color _textColor = (Color)Application.Current.Resources["color-text-secondary"];
        public Color TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }
    }
    public class ProfileStats
    {
        public int EpisodesCount { get; set; }
        public int SeriesCount { get; set; }
        public int HoursSpent { get; set; }
        public int DaysSpent { get; set; }
        public int AchievementsCount { get; set; }
    }
    public class UserEpisodeData
    {
        public int EpisodeId { get; set; }
        public string Title { get; set; }
        public DateTime WatchDate { get; set; }
    }
    public class SeriesStatus
    {
        public string Name { get; set; }
        public string StatusColorName { get; set; }
    }
}
