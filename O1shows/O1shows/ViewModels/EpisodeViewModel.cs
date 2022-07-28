using O1shows.Models;
using O1shows.Services;
using O1shows.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace O1shows.ViewModels
{
    public class EpisodeViewModel
    {
        public IProfileService ProfileService => DependencyService.Get<IProfileService>();
        public Episode Episode { get; set; }
        public Command<int> SelectUserProfileCommand { get; }
        public EpisodeViewModel()
        {
            SelectUserProfileCommand = new Command<int>(ExecuteSelectUserProfile);
        }
        public async void ExecuteSelectUserProfile(int UserProfileId)
        {
            ProfilePage profilePage = new ProfilePage();
            if(UserProfileId != Convert.ToInt32(SecureStorage.GetAsync("userProfileId").Result))
            {
                profilePage.ProfileId = UserProfileId;
                await Shell.Current.CurrentPage.Navigation.PushModalAsync(profilePage);
            }
            else
            {
                await Shell.Current.GoToAsync("//ProfilePage");
            }
        }
    }
}
