using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace O1shows.Services
{
    public interface IApiRequestService
    {
        Task<string> GetAsync(string ControllerName, string ActionName, object paramsObject);
        Task<string> PostAsync(string ControllerName, string ActionName, object model);
    }
    public class ApiRequestService: IApiRequestService
    {
        private static string BaseAddress = "http://192.168.0.9/api";
        public string CreateUrl(string ControllerName, string ActionName)
        {
            return $"{BaseAddress}/{ControllerName}/{ActionName}";
        }
        public async Task<string> GetAsync(string ControllerName, string ActionName, object paramsObject)
        {
            string url = CreateUrl(ControllerName, ActionName);
            RestClient client = new RestClient(url);
            string accessToken = await SecureStorage.GetAsync("accessToken");
            if (accessToken != null)
            {
                client.Authenticator = new JwtAuthenticator(accessToken);
            }
            RestRequest request = new RestRequest();
            request.AddObject(paramsObject);
            RestResponse response = await client.ExecuteGetAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content;
            }
            return "Failed";
        }
        public async Task<string> PostAsync(string ControllerName, string ActionName, object model)
        {
            string url = CreateUrl(ControllerName, ActionName);
            RestClient client = new RestClient(url);
            string accessToken = await SecureStorage.GetAsync("accessToken");
            if (accessToken != null)
            {
                client.Authenticator = new JwtAuthenticator(accessToken);
            }
            RestRequest request = new RestRequest();
            request.AddJsonBody(model);
            RestResponse response = await client.ExecutePostAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content;
            }
            return "Failed";
        }
    }
}
