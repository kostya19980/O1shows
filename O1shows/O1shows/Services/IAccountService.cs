using Newtonsoft.Json;
using O1shows.Models;
using O1shows.ViewModels.AuthViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace O1shows.Services
{
    public interface IAccountService
    {
        Task<UserManagerResponse> RegisterAsync(RegisterModel model);
        Task<UserManagerResponse> LoginAsync(LoginModel model);
    }
    public class AccountService: IAccountService
    {
        public IApiRequestService ApiService => DependencyService.Get<IApiRequestService>();
        public async Task<UserManagerResponse> RegisterAsync(RegisterModel model)
        {
            string ControllerName = "AccountAPI";
            string ActionName = "Register";
            string response = await ApiService.PostAsync(ControllerName, ActionName, model);
            if (response != "Failed")
            {
                return JsonConvert.DeserializeObject<UserManagerResponse>(response);
            }
            return default;
        }
        public async Task<UserManagerResponse> LoginAsync(LoginModel model)
        {
            string ControllerName = "AccountAPI";
            string ActionName = "Login";
            var response = await ApiService.PostAsync(ControllerName, ActionName, model);
            if(response != "Failed")
            {
                UserManagerResponse userManagerResponse = JsonConvert.DeserializeObject<UserManagerResponse>(response);
                return userManagerResponse;
            }
            return default;
        }
    }
    public class UserManagerResponse
    {
        public string UserId { get; set; }
        public int UserProfileId { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Вы не указали Email!")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "Введен некорректный адрес электронной почты!")]
        //[EmailAddress(ErrorMessage = "Некорректный электронный адрес")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес электронной почты!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Вы не указали пароль!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class RegisterModel
    {
        [Required(ErrorMessage = "Вы не указали Email!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес электронной почты!")]
        //[Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Электронный адрес уже используется!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Вы не указали пароль!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Вы не указали пароль!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно!")]
        public string ConfirmPassword { get; set; }
    }
}
