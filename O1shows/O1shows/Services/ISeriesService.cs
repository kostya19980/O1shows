using Newtonsoft.Json;
using O1shows.Models;
using O1shows.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace O1shows.Services
{
    public interface ISeriesService
    {
        Task<SeriesCatalogViewModel> GetSeriesListAsync(int startIndex, int count);
        Task<SeriesViewModel> GetSeriesAsync(int seriesId);
        Task<EpisodeViewModel> GetEpisodeAsync(int EpisodeId);
    }
    public class SeriesService : ISeriesService
    {
        public IApiRequestService ApiService => DependencyService.Get<IApiRequestService>();
        public async Task<SeriesCatalogViewModel> GetSeriesListAsync(int startIndex, int count)
        {
            string ControllerName = "SeriesAPI";
            string ActionName = "GetSeriesList";
            var parameters = new
            {
                startIndex = startIndex,
                count = count
            };
            string response = await ApiService.GetAsync(ControllerName, ActionName, parameters);
            if(response != "Failed")
            {
                SeriesCatalogViewModel model = JsonConvert.DeserializeObject<SeriesCatalogViewModel>(response);
                double width = (Application.Current.MainPage.Width - 20) / 2;
                foreach (var series in model.SeriesListView)
                {
                    series.PicturePath = "http://192.168.0.9/" + series.PicturePath.Replace("normal", "small");
                    series.SeriesBlockWidth = width;
                    series.SeriesRaiting = new SeriesRaiting(series.Raiting, "IMDB");
                }
                return model;
            }
            return default;
        }
        public async Task<SeriesViewModel> GetSeriesAsync(int seriesId)
        {
            string ControllerName = "SeriesAPI";
            string ActionName = "GetSeries";
            var parameters = new
            {
                SeriesId = seriesId,
            };
            string response = await ApiService.GetAsync(ControllerName, ActionName, parameters);
            if (response != "Failed")
            {
                SeriesViewModel model = JsonConvert.DeserializeObject<SeriesViewModel>(response);
                model.Series.PicturePath = "http://192.168.0.9/" + model.Series.PicturePath;
                model.Raitings = new List<SeriesRaiting>()
                {
                    new SeriesRaiting(model.Series.Raiting.Raiting, "Рейтинг 01shows"),
                    new SeriesRaiting(model.Series.Raiting.IMDB, "Рейтинг IMDB"),
                    new SeriesRaiting(model.Series.Raiting.Kinopoisk, "Рейтинг Кинопоиск"),
                };
                model.Genres = string.Join(", ", model.Series.SeriesGenres.Select(p => p.Genre.Name));
                foreach(Season season in model.Seasons)
                {
                    season.IsFirstCheck = true;
                    season.IsChecked = season.WatchedCount == season.Episodes.Count();
                    season.IsFirstCheck = false;
                    foreach(EpisodeCheckBox episode in season.Episodes)
                    {
                        episode.IsFirstCheck = false;
                    }
                }
                return model;
            }
            return default;
        }
        public async Task<EpisodeViewModel> GetEpisodeAsync(int EpisodeId)
        {
            string ControllerName = "SeriesAPI";
            string ActionName = "GetEpisode";
            var parameters = new
            {
                EpisodeId = EpisodeId,
            };
            string response = await ApiService.GetAsync(ControllerName, ActionName, parameters);
            if (response != "Failed")
            {
                EpisodeViewModel model = JsonConvert.DeserializeObject<EpisodeViewModel>(response);
                foreach(Comment comment in model.Episode.Comments)
                {
                    comment.UserProfile.ImageSrc = "http://192.168.0.9/" + comment.UserProfile.ImageSrc;
                }
                return model;
            }
            return default;
        }
    }
}
