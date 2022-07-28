using O1shows.Services;
using O1shows.ViewModels;
using O1shows.Views;
using O1shows.Views.Account;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace O1shows
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<ApiRequestService>();
            DependencyService.Register<AccountService>();
            DependencyService.Register<ProfileService>();
            DependencyService.Register<SeriesService>();
            DependencyService.Get<IStatusBarStyleManager>().SetTransparent();
            bool isLoogged = Convert.ToBoolean(SecureStorage.GetAsync("isLogged").Result);
            if (isLoogged)
            {
                MainPage = new AppShell();
                Shell.Current.GoToAsync("//SeriesCatalogPage");
            }
            else
            {
                MainPage = new AppShell();
                Shell.Current.GoToAsync("//LoginPage");
            }
        }
        protected override void OnStart()
        {
            //await Shell.Current.GoToAsync("//ProfilePage");
            ////await Shell.Current.GoToAsync($"//{nameof(ProfilePage)}");
            //string accessToken = await SecureStorage.GetAsync("accessToken");
            //if (accessToken != null)
            //{
            //    IsLogged = true;
            //}
            //else
            //{
            //    IsLogged = false;
            //}
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
