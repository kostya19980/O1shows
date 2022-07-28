using O1shows.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace O1shows.Views.Messenger
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendsView : ContentPage
    {
        private FriendsViewModel viewModel;
        public FriendsView()
        {
            viewModel = new FriendsViewModel();
            BindingContext = viewModel;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}