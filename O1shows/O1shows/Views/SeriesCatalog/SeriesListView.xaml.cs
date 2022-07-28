using O1shows.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace O1shows.Views.SeriesCatalog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeriesListView : ContentPage
    {
        private SeriesCatalogViewModel viewModel;
        public SeriesListView()
        {
            viewModel = new SeriesCatalogViewModel();
            BindingContext = viewModel;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
        private void OnScrolledSeriesList(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            if (scrollingSpace <= e.ScrollY + 1)
            {
                int totalCount = viewModel.SeriesListView.Count;
                int loadedCount = viewModel.LoadedSeriesList.Count;
                if (totalCount > loadedCount)
                {
                    foreach (var item in viewModel.SeriesListView.Skip(loadedCount).Take(16))
                    {
                        viewModel.LoadedSeriesList.Add(item);
                    }
                }
            }
        }
    }
}