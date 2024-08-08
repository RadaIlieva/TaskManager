using Newtonsoft.Json;
using System.Text;
using TaskManagerProject.DTOs;
using TaskManagerProject.Services.Interfaces;

public class AccountService : IAccountService
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;
    private readonly ILogger<AccountService> logger;

    public AccountService(HttpClient httpClient, IConfiguration configuration, ILogger<AccountService> logger)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
        this.logger = logger;
    }

    public async Task<bool> RegisterAsync(RegisterUserDto registerUserDto)
    {
        var url = configuration["ApiUrls:Account:Register"];
        logger.LogInformation($"Sending request to {url}");
        var jsonContent = JsonConvert.SerializeObject(registerUserDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, content);
        logger.LogInformation($"Response status code: {response.StatusCode}");

        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            logger.LogError($"Registration failed: {responseContent}");
        }

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