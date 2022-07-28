using O1shows.Models;
using O1shows.Services;
using O1shows.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace O1shows.ViewModels
{
    public class SeriesViewModel : BaseViewModel
    {
        public IProfileService ProfileService => DependencyService.Get<IProfileService>();
        public ISeriesService SeriesService => DependencyService.Get<ISeriesService>();
        public IStatusBarStyleManager StatusBarStyleManager => DependencyService.Get<IStatusBarStyleManager>();
        public Series Series { get; set; }
        public string ReleaseDates { get; set; }
        public string Genres { get; set; }
        public int SeasonSize { get; set; }
        public int _userRaiting;
        public int UserRaiting
        {
            get { return _userRaiting; }
            set 
            { 
                SetProperty(ref _userRaiting, value);
                UserRaitingColor = RaitingColor.DetermineColor(value);
            }
        }
        public string _userRaitingColor;
        public string UserRaitingColor
        {
            get { return _userRaitingColor; }
            set { SetProperty(ref _userRaitingColor, value); }
        }
        public string StatusColor { get; set; }
        public string _currentWatchStatus;
        public string CurrentWatchStatus
        {
            get { return _currentWatchStatus; }
            set 
            { 
                SetProperty(ref _currentWatchStatus, value);
                if(value == "Просмотрено")
                {
                    CheckAllButton = CheckAllButtons[0];
                }
                else
                {
                    CheckAllButton = CheckAllButtons[1];
                }
            }
        }
        public List<WatchStatusButton> WatchStatusButtons { get; set; }
        public WatchStatusButton CurrentWatchStatusButton { get; set; }
        public List<Button> CheckAllButtons { get; set; }
        public Button CheckAllButton { get; set; }
        public List<string> WatchStatuses { get; set; }
        public List<Season> Seasons { get; set; }
        public List<SeriesRaiting> Raitings { get; set; }
        public List<Tab> Tabs { get; set; }
        public Command ToogleTab { get; }
        public Command SlideRaiting { get; }
        public Command SelectSeriesRaiting { get; }
        public Command CheckAllEpisodes { get; }
        public Command<int> SelectEpisodeCommand { get; }
        public SeriesViewModel()
        {
            Tabs = new List<Tab>()
            {
                new Tab {Index = 0, Name = "О сериале", IsChecked = true },
                new Tab {Index = 1, Name = "Эпизоды", IsChecked = false },
            };
            CheckAllButtons = new List<Button>()
            {
                new Button
                {
                    Text = "Снять отметки",
                    BackgroundColor = (Color)Application.Current.Resources["color-accent-purple"],
                    TextColor = (Color)Application.Current.Resources["color-text-heading"]
                },
                new Button
                {
                    Text = "Отметить все серии",
                    BackgroundColor = (Color)Application.Current.Resources["grey-10"],
                    TextColor = (Color)Application.Current.Resources["color-text-secondary"]
                }
            };
            WatchStatusButtons = new List<WatchStatusButton>()
            {
                new WatchStatusButton {Icon = "add.xml", Status = "Добавить"},
                new WatchStatusButton {Icon = "eye_open.xml", Status = "Смотрю"},
                new WatchStatusButton {Icon = "eye_open.xml", Status = "Запланировано"},
                new WatchStatusButton {Icon = "eye_open.xml", Status = "Отложено"},
                new WatchStatusButton {Icon = "eye_close.xml", Status = "Брошено"},
                new WatchStatusButton {Icon = "success.xml", Status = "Просмотрено"},
            };
            ToogleTab = new Command<Tab>(ExecuteToogleTab);
            SlideRaiting = new Command<int>(ExecuteSlideRaiting);
            SelectSeriesRaiting = new Command(ExecuteSelectSeriesRaiting);
            CheckAllEpisodes = new Command(ExecuteCheckAllEpisodes);
            SelectEpisodeCommand = new Command<int>(ExecuteSelectEpisode);
        }
        public async void ExecuteSelectEpisode(int episodeId)
        {
            EpisodeViewModel model = await SeriesService.GetEpisodeAsync(episodeId);
            await Shell.Current.CurrentPage.Navigation.PushModalAsync(new EpisodePage(model));
        }
        public void ExecuteCheckAllEpisodes()
        {
            bool IsCheckAll = !(CurrentWatchStatus == "Просмотрено");
            foreach (Season season in Seasons)
            {
                if (season.IsChecked != IsCheckAll)
                {
                    season.IsChecked = IsCheckAll;
                }
            }
        }
        public async void ExecuteSelectSeriesRaiting()
        {
            var updatedRaiting = await ProfileService.SelectSeriesRaiting(UserRaiting, Series.Id);
            Raitings[0].Value = updatedRaiting;
        }
        public void ExecuteSlideRaiting(int value)
        {
            UserRaitingColor = RaitingColor.DetermineColor(value);
        }
        public void ExecuteToogleTab(Tab tab)
        {
            if (!tab.IsChecked)
            {
                Tabs.FirstOrDefault(x => x.IsChecked).IsChecked = false;
                tab.IsChecked = true;
            }
        }
        public string IsWatchCompleted()
        {
            int WatchedCount = Seasons.Sum(x => x.Episodes.Count(e => e.IsChecked && e.Episode.EpisodeNumber != 0));
            int TotalCount = Seasons.Sum(x => x.Episodes.Count(e => e.Episode.EpisodeNumber != 0));
            if (WatchedCount == TotalCount)
            {
                return "Просмотрено";
            }
            return "Смотрю";
        }
    }
    public class Button
    {
        public string Text { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
    }
    public class WatchStatusButton
    {
        public string Icon { get; set; }
        public string Status { get; set; }
    }
    public class Season : BaseViewModel
    {
        public Season()
        {
            IsFirstCheck = true;
        }
        public bool _isFirstCheck;
        public bool IsFirstCheck
        {
            get { return _isFirstCheck; }
            set
            {
                SetProperty(ref _isFirstCheck, value);
            }
        }
        public bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set 
            { 
                SetProperty(ref _isChecked, value);
                if (!IsFirstCheck)
                {
                    CheckedSeason(value);
                }
            }
        }
        public int WatchedCount { get; set; }
        public int SeasonNumber { get; set; }
        public int _seasonSize;
        public int SeasonSize
        {
            get 
            {
                return Episodes.Count(x => x.Episode.EpisodeNumber != 0);
            }
            set
            {
                SetProperty(ref _seasonSize, value);
            }
        }
        public List<EpisodeCheckBox> Episodes { get; set; }
        public async void CheckedSeason(bool isChecked)
        {
            List<int> CheckedIds = new List<int>();
            foreach (EpisodeCheckBox episode in Episodes)
            {
                if (episode.IsChecked != isChecked)
                {
                    episode.IsOrdinaryChecked = false;
                    episode.IsChecked = isChecked;
                    CheckedIds.Add(episode.Episode.Id);
                }
            }
            WatchedCount = Episodes.Count(e => e.IsChecked && e.Episode.EpisodeNumber != 0);
            await DependencyService.Get<IProfileService>().CheckEpisodes(CheckedIds.ToArray(), Episodes[0].Episode.SeriesId);
        }
    }
    public class EpisodeCheckBox : BaseViewModel
    {
        public EpisodeCheckBox()
        {
            IsFirstCheck = true;
            IsOrdinaryChecked = true;
        }
        public bool _isFirstCheck;
        public bool IsFirstCheck
        {
            get { return _isFirstCheck; }
            set
            {
                SetProperty(ref _isFirstCheck, value);
            }
        }
        public bool _isOrdinaryChecked;
        public bool IsOrdinaryChecked
        {
            get { return _isOrdinaryChecked; }
            set
            {
                SetProperty(ref _isOrdinaryChecked, value);
            }
        }
        public Episode Episode { get; set; }
        public bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                SetProperty(ref _isChecked, value);
                if (IsOrdinaryChecked)
                {
                    if (!IsFirstCheck)
                    {
                        int[] CheckedIds = new int[] { Episode.Id };
                        DependencyService.Get<IProfileService>().CheckEpisodes(CheckedIds, Episode.SeriesId);
                    }
                }
                else
                {
                    IsOrdinaryChecked = true;
                }
            }
        }
    }
    public class Tab: BaseViewModel
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(ref _isChecked, value); }
        }
    }
}
