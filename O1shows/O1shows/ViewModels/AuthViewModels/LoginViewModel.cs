using O1shows.Services;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace O1shows.ViewModels.AuthViewModels
{
    public class LoginViewModel: BaseViewModel
    {
        public IAccountService AccountService => DependencyService.Get<IAccountService>();
        [Required(ErrorMessage = "Вы не указали Email!")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "Введен некорректный адрес электронной почты!")]
        //[EmailAddress(ErrorMessage = "Некорректный электронный адрес")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес электронной почты!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Вы не указали пароль!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(async() =>
                {
                    LoginModel model = new LoginModel
                    {
                        Email = Email,
                        Password = Password
                    };
                   var result = await AccountService.LoginAsync(model);
                    if(result != null)
                    {
                        if (result.IsSuccess)
                        {
                            await SecureStorage.SetAsync("isLogged", "true");
                            await SecureStorage.SetAsync("accessToken", result.Message);
                            await SecureStorage.SetAsync("userProfileId", result.UserProfileId.ToString());
                            await Shell.Current.GoToAsync("//ProfilePage");
                        }
                    }
                });
            }
        }
    }
}
