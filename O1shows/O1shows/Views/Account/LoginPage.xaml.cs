using O1shows.ViewModels.AuthViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace O1shows.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel loginModel;
        public LoginPage()
        {
            loginModel = new LoginViewModel();
            BindingContext = loginModel;
            InitializeComponent();
        }
    }
}