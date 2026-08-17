using SpohnStory.Models;
using SpohnStory.Configuration;

namespace SpohnStory.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse?> LoginAsync(string username, string password);
        Task<AuthenticationResponse?> RegisterAsync(string username, string password);
        void SaveToken(string token);
        string? GetTokenAsync();
        bool HasValidToken();
        void ClearToken();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApiClient _apiClient;
        private readonly IConfigurationService _configService;
        private readonly ILoggingService _logger;
        private string? _currentToken;

        public AuthenticationService(
            IApiClient apiClient,
            IConfigurationService configService,
            ILoggingService logger)
        {
            _apiClient = apiClient;
            _configService = configService;
            _logger = logger;

            // Load stored token on initialization
            var config = _configService.GetConfiguration();
            _currentToken = config.AuthToken;
        }

        public async Task<AuthenticationResponse?> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogError("Username and password are required");
                return new AuthenticationResponse 
                { 
                    Success = false, 
                    Message = "Username and password are required" 
                };
            }

            try
            {
                var response = await _apiClient.LoginAsync(username, password);
                if (response?.Success == true && !string.IsNullOrEmpty(response.Token))
                {
                    SaveToken(response.Token);
                    _logger.Log($"User logged in successfully: {username}", "Login");
                    return response;
                }

                _logger.LogError($"Login failed for user: {username}");
                return response ?? new AuthenticationResponse 
                { 
                    Success = false, 
                    Message = "Login failed" 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Login process failed", ex);
                return new AuthenticationResponse 
                { 
                    Success = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<AuthenticationResponse?> RegisterAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogError("Username and password are required");
                return new AuthenticationResponse 
                { 
                    Success = false, 
                    Message = "Username and password are required" 
                };
            }

            try
            {
                var response = await _apiClient.RegisterAsync(username, password);
                if (response?.Success == true)
                {
                    _logger.Log($"User registered successfully: {username}", "Register");
                    return response;
                }

                _logger.LogError($"Registration failed for user: {username}");
                return response ?? new AuthenticationResponse 
                { 
                    Success = false, 
                    Message = "Registration failed" 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Registration process failed", ex);
                return new AuthenticationResponse 
                { 
                    Success = false, 
                    Message = ex.Message 
                };
            }
        }

        public void SaveToken(string token)
        {
            _currentToken = token;
            var config = _configService.GetConfiguration();
            config.AuthToken = token;
            config.LastLogin = DateTime.Now;
            _configService.SaveConfiguration(config);
            _logger.Log("Token saved to configuration", "TokenManagement");
        }

        public string? GetTokenAsync()
        {
            return _currentToken;
        }

        public bool HasValidToken()
        {
            return !string.IsNullOrEmpty(_currentToken);
        }

        public void ClearToken()
        {
            _currentToken = null;
            var config = _configService.GetConfiguration();
            config.AuthToken = null;
            _configService.SaveConfiguration(config);
            _logger.Log("Token cleared", "TokenManagement");
        }
    }
}
