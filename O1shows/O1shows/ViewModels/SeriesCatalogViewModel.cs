using O1shows.Models;
using O1shows.Services;
using O1shows.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace O1shows.ViewModels
{
    public class SeriesCatalogViewModel : BaseViewModel
    {
        public ISeriesService SeriesService => DependencyService.Get<ISeriesService>();
        public IProfileService ProfileService => DependencyService.Get<IProfileService>();
        public List<SeriesCatalogItem> _seriesListView;
        public List<SeriesCatalogItem> SeriesListView
        {
            get { return _seriesListView; }
            set { SetProperty(ref _seriesListView, value); }
        }
        public ObservableCollection<SeriesCatalogItem> _loadedSeriesList;
        public ObservableCollection<SeriesCatalogItem> LoadedSeriesList
        {
            get { return _loadedSeriesList; }
            set { SetProperty(ref _loadedSeriesList, value); }
        }
        public List<SeriesFilter> _filters;
        public List<SeriesFilter> Filters
        {
            get { return _filters; }
            set { SetProperty(ref _filters, value); }
        }
        public Command GetSeriesListCommand { get; }
        public Command GetRecommendationsCommand { get; }
        public Command<int> SelectSeriesCommand { get; }
        public SeriesCatalogViewModel()
        {
            GetSeriesListCommand = new Command(async () => await ExecuteGetSeriesList());
            GetRecommendationsCommand = new Command(async () => await ExecuteGetRecommendationsCommand());
            SelectSeriesCommand = new Command<int>(ExecuteSelectSeries);
        }
        public async Task ExecuteGetRecommendationsCommand()
        {
            IsBusy = true;

            try
            {
                int profileId = Convert.ToInt32(SecureStorage.GetAsync("userProfileId").Result);
                SeriesCatalogViewModel model = await ProfileService.GetRecommendations(profileId);
                SeriesListView = model.SeriesListView;
                LoadedSeriesList = new ObservableCollection<SeriesCatalogItem>();
                foreach (var item in model.SeriesListView.Take(16))
                {
                    LoadedSeriesList.Add(item);
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
        public async Task ExecuteGetSeriesList()
        {
            IsBusy = true;

            try
            {
                SeriesCatalogViewModel model = await SeriesService.GetSeriesListAsync(1, 100);
                SeriesListView = model.SeriesListView;
                Filters = model.Filters;
                LoadedSeriesList = new ObservableCollection<SeriesCatalogItem>();
                foreach (var item in model.SeriesListView.Take(16))
                {
                    LoadedSeriesList.Add(item);
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
        public async void ExecuteSelectSeries(int seriesId)
        {
            SeriesViewModel model = await SeriesService.GetSeriesAsync(seriesId);
            await Shell.Current.CurrentPage.Navigation.PushModalAsync(new SeriesPage(model));
        }
        public void OnAppearing()
        {
            IsBusy = LoadedSeriesList == null;
        }
    }
    public class SeriesCatalogItem
    {
        public int SeriesId { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string PicturePath { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public double SeriesBlockWidth { get; set; }
        public float Raiting { get; set; }
        public int? PotentialRating { get; set; }
        public SeriesRaiting SeriesRaiting { get; set; }
    }
    public class SeriesRaiting: BaseViewModel
    {
        public SeriesRaiting(float value, string name)
        {
            Value = value;
            Name = name;
            Color = RaitingColor.DetermineColor(value);
        }
        public string Color { get; set; }
        public string Name { get; set; }
        public float _value;
        public float Value
        {
            get { return _value; }
            set 
            { 
                SetProperty(ref _value, value);
                Color = RaitingColor.DetermineColor(value);
            }
        }
    }
    static class RaitingColor
    {
        public static string DetermineColor(float raiting)
        {
            if (raiting >= 0 && raiting < 2)
            {
                return "#ff0000";
            }
            else if (raiting >= 2 && raiting < 4)
            {
                return "#f06f05";
            }
            else if (raiting >= 4 && raiting < 6)
            {
                return "#F9CC00";
            }
            else if (raiting >= 6 && raiting < 8)
            {
                return "#70b502";
            }
            else
            {
                return "#228B22";
            }
        }
    }
    public class SeriesFilter
    {
        public string Name { get; set; }
        public List<string> Values { get; set; }
    }
    //public class SeriesFilters
    //{
    //    public IEnumerable<Genre> Genres { get; set; }
    //    public IEnumerable<Country> Countries { get; set; }
    //    public IEnumerable<Channel> Channels { get; set; }
    //    public int[] Years { get; set; }
    //    public IEnumerable<Status> Statuses { get; set; }
    //    public string[] Raitings { get; set; }
    //}
}
