using SpohnStory.Models;
using SpohnStory.Configuration;

namespace SpohnStory.Services
{
    public interface IApiClient
    {
        Task<AuthenticationResponse?> LoginAsync(string username, string password);
        Task<AuthenticationResponse?> RegisterAsync(string username, string password);
        Task<string?> GetWzKeyAsync(string token);
        Task<bool> ValidateTokenAsync(string token);
    }

    public class ApiClient : IApiClient
    {
        private readonly IConfigurationService _configService;
        private readonly ILoggingService _logger;
        private readonly HttpClient _httpClient;

        public ApiClient(IConfigurationService configService, ILoggingService logger)
        {
            _configService = configService;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task<AuthenticationResponse?> LoginAsync(string username, string password)
        {
            try
            {
                var config = _configService.GetConfiguration();
                if (string.IsNullOrEmpty(config.ApiUrl))
                {
                    _logger.LogError("API URL not configured");
                    return new AuthenticationResponse { Success = false, Message = "API URL not configured" };
                }

                var loginUrl = $"{config.ApiUrl}/api/login";
                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(new { username, password }),
                    System.Text.Encoding.UTF8,
                    "application/json");

                _logger.Log($"Attempting login to {loginUrl}", "Authentication");

                var response = await _httpClient.PostAsync(loginUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = System.Text.Json.JsonSerializer.Deserialize<AuthenticationResponse>(responseContent);
                    if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
                    {
                        _logger.Log($"Login successful for user: {username}", "Authentication");
                        return result;
                    }
                }

                _logger.LogError($"Login failed: {response.StatusCode} - {responseContent}");
                return new AuthenticationResponse 
                { 
                    Success = false, 
                    Message = $"Login failed: {response.StatusCode}" 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Login request failed", ex);
                return new AuthenticationResponse 
                { 
                    Success = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<AuthenticationResponse?> RegisterAsync(string username, string password)
        {
            try
            {
                var config = _configService.GetConfiguration();
                if (string.IsNullOrEmpty(config.ApiUrl))
                {
                    _logger.LogError("API URL not configured");
                    return new AuthenticationResponse { Success = false, Message = "API URL not configured" };
                }

                var registerUrl = $"{config.ApiUrl}/api/register";
                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(new { username, password }),
                    System.Text.Encoding.UTF8,
                    "application/json");

                _logger.Log($"Attempting registration", "Authentication");

                var response = await _httpClient.PostAsync(registerUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = System.Text.Json.JsonSerializer.Deserialize<AuthenticationResponse>(responseContent);
                    if (result?.Success == true)
                    {
                        _logger.Log($"Registration successful for user: {username}", "Authentication");
                        return result;
                    }
                }

                _logger.LogError($"Registration failed: {response.StatusCode} - {responseContent}");
                return new AuthenticationResponse 
                { 
                    Success = false, 
                    Message = $"Registration failed: {response.StatusCode}" 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Registration request failed", ex);
                return new AuthenticationResponse 
                { 
                    Success = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<string?> GetWzKeyAsync(string token)
        {
            try
            {
                var config = _configService.GetConfiguration();
                if (string.IsNullOrEmpty(config.ApiUrl))
                {
                    _logger.LogError("API URL not configured");
                    return null;
                }

                var keyUrl = $"{config.ApiUrl}/api/key";
                using (var request = new HttpRequestMessage(HttpMethod.Get, keyUrl))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    _logger.Log("Requesting WZ key", "KeyRequest");
                    var response = await _httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(responseContent);

                        if (result?.TryGetValue("key", out var key) == true)
                        {
                            _logger.Log("WZ key retrieved successfully", "KeyRequest");
                            return key?.ToString();
                        }
                    }

                    _logger.LogError($"Failed to retrieve WZ key: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("WZ key request failed", ex);
            }

            return null;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var config = _configService.GetConfiguration();
                if (string.IsNullOrEmpty(config.ApiUrl))
                {
                    return false;
                }

                var validateUrl = $"{config.ApiUrl}/api/validate";
                using (var request = new HttpRequestMessage(HttpMethod.Get, validateUrl))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await _httpClient.SendAsync(request);
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
