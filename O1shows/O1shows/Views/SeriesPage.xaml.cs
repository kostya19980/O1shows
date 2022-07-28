using DarkIce.Toolkit.Core.Utilities;
using O1shows.Models;
using O1shows.Services;
using O1shows.ViewModels;
using System;
using System.Linq;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace O1shows.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeriesPage : ContentPage
    {
        private SeriesViewModel viewModel;
        public SeriesPage(SeriesViewModel model)
        {
            viewModel = model;
            BindingContext = viewModel;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ScrollTitle();
            InitWatchStatuses();
        }
        public void InitWatchStatuses()
        {
            WatchStatusDropdown.ItemsSource = viewModel.WatchStatuses;
            WatchStatusDropdown.SelectedIndex = viewModel.WatchStatuses.IndexOf(viewModel.CurrentWatchStatus);
            BindCurrentWatchStatusButton(viewModel.CurrentWatchStatus);
        }
        private void ScrollTitle()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                var scrollToEndAnim = new Animation(
                    callback: x => titleScroll.ScrollToAsync(x, 0, animated: false),
                    start: titleScroll.ScrollX,
                    end: titleScroll.Width);
                scrollToEndAnim.Commit(
                    owner: this,
                    name: "Scroll",
                    length: 5500,
                    easing: Easing.CubicIn);
                Device.StartTimer(TimeSpan.FromSeconds(8), () =>
                {
                    var scrollToBeginAnim = new Animation(
                        callback: x => titleScroll.ScrollToAsync(x, 0, animated: false),
                        start: titleScroll.ScrollX,
                        end: 0);
                    scrollToBeginAnim.Commit(
                        owner: this,
                        name: "Scroll",
                        length: 5500,
                        easing: Easing.CubicIn);
                    Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                    {
                        ScrollTitle();
                        return false;
                    });
                    return false;
                });
                //titleScroll.FadeTo(1, 200, Easing.CubicIn);
                //titleScroll.ScrollToAsync(titleScroll.Width, 0, true);
                return false;
            });
            //Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            //{
            //    titleScroll.ScrollToAsync(0, 0, true);
            //    return false;
            //});
        }
        //private void StarAnimate()
        //{
        //    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        //    {
        //        int statusBarHeight = viewModel.StatusBarStyleManager.GetHeight();
        //        totalScroll.ScrollToAsync(0, 220, false);
        //        return false;
        //    });
        //}
        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            slider.Value = Math.Round(e.NewValue);
        }

        private void Expander_Tapped(object sender, EventArgs e)
        {
            Expander expander = sender as Expander;
            if (expander.IsExpanded)
            {
                description_shadow.IsVisible = false;
                expander_button.Text = "Скрыть информацию";
                description.HeightRequest = -1;
            }
            else
            {
                description_shadow.IsVisible = true;
                expander_button.Text = "Подробная информация";
                description.HeightRequest = 110;
            }
        }
        public async void ClosingPage(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    if (pageContent.TranslationY + e.TotalY < 0)
                    {
                        pageContent.TranslationY = 0;
                    }
                    else
                    {
                        pageContent.TranslationY += e.TotalY;
                    }
                    break;
                case GestureStatus.Completed:
                    if (pageContent.TranslationY > 250)
                    {
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await pageContent.TranslateTo(0, 0);
                    }
                    break;
            }
        }
        public async void MoveContentUp(object sender, PanUpdatedEventArgs e)
        {

            StackLayout layout = sender as StackLayout;
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    if (layout.TranslationY + e.TotalY < 0)
                    {
                        if (layout.TranslationY + e.TotalY > -seriesCover.HeightRequest + titleBlock.Height)
                        {
                            layout.TranslationY += e.TotalY;
                        }
                        else
                        {
                            layout.TranslationY = -seriesCover.HeightRequest + titleBlock.Height;
                        }
                    }
                    else
                    {
                        layout.TranslationY = 0;
                    }
                    break;
                case GestureStatus.Completed:
                    if (layout.TranslationY > -seriesCover.HeightRequest/3)
                    {
                        await layout.TranslateTo(0, 0);
                    }
                    else
                    {
                        await layout.TranslateTo(0, -seriesCover.HeightRequest + titleBlock.Height);
                    }
                    break;
            }
        }

        private async void ClosePage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void WatchStatusDropdown_ItemSelected(object sender, Elements.Dropdown.ItemSelectedEventArgs e)
        {
            string selectedStatus = viewModel.WatchStatuses[e.SelectedIndex];
            if(selectedStatus != viewModel.CurrentWatchStatus)
            {
                viewModel.CurrentWatchStatus = selectedStatus;
                BindCurrentWatchStatusButton(selectedStatus);
                await viewModel.ProfileService.SelectWatchStatus(viewModel.Series.Id, selectedStatus);
            }
        }
        public void BindCurrentWatchStatusButton(string status)
        {
            if (status == "Не смотрю")
            {
                viewModel.CurrentWatchStatusButton = viewModel.WatchStatusButtons.FirstOrDefault(x => x.Status == "Добавить");
            }
            else
            {
                viewModel.CurrentWatchStatusButton = viewModel.WatchStatusButtons.FirstOrDefault(x => x.Status == status);
            }
        }

        private void Episode_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox check = sender as CheckBox;
            EpisodeCheckBox episode = check.BindingContext as EpisodeCheckBox;
            Season season = viewModel.Seasons.FirstOrDefault(x => x.SeasonNumber == episode.Episode.SeasonNumber);
            season.WatchedCount = season.Episodes.Count(ep => ep.IsChecked && ep.Episode.EpisodeNumber != 0);
            season.IsFirstCheck = true;
            if (season.WatchedCount == season.SeasonSize)
            {
                season.IsChecked = true;
            }
            else
            {
                season.IsChecked = false;
            }
            season.IsFirstCheck = false;
            WatchStatusDropdown.SelectedIndex = viewModel.WatchStatuses.IndexOf(viewModel.IsWatchCompleted());
        }
    }
}