using O1shows.Models;
using O1shows.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace O1shows.ViewModels
{
    public class FriendsViewModel:BaseViewModel
    {
        public IProfileService ProfileService => DependencyService.Get<IProfileService>();
        public ObservableCollection<UserProfile> _friends;
        public ObservableCollection<UserProfile> Friends
        {
            get { return _friends; }
            set { SetProperty(ref _friends, value); }
        }
        public Command GetFriendsListCommand { get; }
        public FriendsViewModel()
        {
            GetFriendsListCommand = new Command(async () => await ExecuteGetFriendsList());
        }
        public async Task ExecuteGetFriendsList()
        {
            IsBusy = true;

            try
            {
                FriendsViewModel model = await ProfileService.GetFriends();
                Friends = model.Friends;
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
        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
