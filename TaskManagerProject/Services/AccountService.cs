using TaskManagerProject.DTOs;
using TaskManagerProject.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerProject.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public AccountService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task<bool> RegisterAsync(RegisterUserDto registerUserDto)
        {
            var url = configuration["ApiUrls:Account:Register"];
            var jsonContent = JsonConvert.SerializeObject(registerUserDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<string> LoginAsync(LoginUserDto loginUserDto)
        {
            var url = configuration["ApiUrls:Account:Login"];
            var jsonContent = JsonConvert.SerializeObject(loginUserDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
                return responseObject.Token;
            }

            return null;
        }
    }
}
