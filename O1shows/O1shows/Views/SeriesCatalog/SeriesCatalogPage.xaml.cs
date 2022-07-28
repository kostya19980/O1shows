using DarkIce.Toolkit.Core.Utilities;
using O1shows.Elements.Dropdown;
using O1shows.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace O1shows.Views.SeriesCatalog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeriesCatalogPage : TabbedPage
    {
        //private SeriesCatalogViewModel viewModel;
        public SeriesCatalogPage()
        {
            //viewModel = new SeriesCatalogViewModel();
            //BindingContext = viewModel;
            InitializeComponent();
        }
        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    //await Sheet.CloseSheet();
        //    //viewModel.OnAppearing();
        //}
        //public void CreateFilters()
        //{
        //    if(filtersBlock.Children.Count == 0)
        //    {
        //        Color backgroundColor = (Color)Application.Current.Resources["grey-100"];
        //        Color textColor = (Color)Application.Current.Resources["color-text-heading"];
        //        foreach (SeriesFilter filter in viewModel.Filters)
        //        {
        //            Frame frame = new Frame()
        //            {
        //                CornerRadius = 10,
        //                Padding = 0,
        //                Margin = new Thickness(0, 8, 0, 0)
        //            };
        //            filter.Values.Insert(0, filter.Name);
        //            Dropdown dropdown = new Dropdown()
        //            {
        //                ItemsSource = filter.Values,
        //                HeightRequest = 40
        //            };
        //            dropdown.BackgroundColor = backgroundColor;
        //            frame.Content = dropdown;
        //            filtersBlock.Children.Add(frame);
        //        }
        //    }
        //}
        //private async void FilterSeries(object sender, EventArgs e)
        //{
        //    if (Sheet.IsOpened)
        //    {
        //        try
        //        {
        //            await Sheet.CloseSheet();
        //        }
        //        catch (Exception ex)
        //        {
        //            ex.Log();
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            await Sheet.OpenSheet();
        //            //CreateFilters();
        //        }
        //        catch (Exception ex)
        //        {
        //            ex.Log();
        //        }
        //    }
        //}
        //private void ClearSearchEntry(object sender, EventArgs e)
        //{         
        //    search.Text = "";
        //}
        //private void OnTextChanged(object sender, FocusEventArgs e)
        //{
        //    if (search.Text != "")
        //    {
        //        clearSearchButton.IsVisible = true;
        //    }
        //    else
        //    {
        //        clearSearchButton.IsVisible = false;
        //    }
        //}
        //private void OnScrolledSeriesList(object sender, ScrolledEventArgs e)
        //{
        //    ScrollView scrollView = sender as ScrollView;
        //    double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

        //    if (scrollingSpace <= e.ScrollY + 1)
        //    {
        //        int totalCount = viewModel.SeriesListView.Count;
        //        int loadedCount = viewModel.LoadedSeriesList.Count;
        //        if (totalCount > loadedCount)
        //        {
        //            foreach (var item in viewModel.SeriesListView.Skip(loadedCount).Take(16))
        //            {
        //                viewModel.LoadedSeriesList.Add(item);
        //            }
        //        }
        //    }
        //}
    }
}