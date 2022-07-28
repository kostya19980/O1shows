using O1shows.ViewModels.AuthViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace O1shows.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private RegisterViewModel registerModel;
        public RegisterPage()
        {
            registerModel = new RegisterViewModel();
            BindingContext = registerModel;
            InitializeComponent();
        }
    }
}