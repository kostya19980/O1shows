using O1shows.Services;
using O1shows.ViewModels;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace O1shows.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private UserProfileViewModel userProfileViewModel;
        public int ProfileId { get; set; }
        public ProfilePage()
        {
            userProfileViewModel = new UserProfileViewModel();
            BindingContext = userProfileViewModel;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(ProfileId != 0)
            {
                userProfileViewModel.Id = ProfileId;
            }
            userProfileViewModel.OnAppearing();

        }
        private void OnScrolledSeriesList(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            if (scrollingSpace <= e.ScrollY + 1)
            {
                int totalCount = userProfileViewModel.CurrentStatusTab.SeriesList.Count;
                int loadedCount = userProfileViewModel.CurrentStatusTab.LoadedSeriesList.Count;
                if (totalCount > loadedCount)
                {
                    foreach(var item in userProfileViewModel.CurrentStatusTab.SeriesList.Skip(loadedCount).Take(6))
                    {
                        userProfileViewModel.CurrentStatusTab.LoadedSeriesList.Add(item);
                    }
                }
            }
        }
    }
}