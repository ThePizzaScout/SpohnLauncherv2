using SpohnStory.Models;
using SpohnStory.Configuration;
using System.Diagnostics;

namespace SpohnStory.Services
{
    public interface ILaunchService
    {
        Task<(bool Success, string Message, int? ProcessId)> LaunchGameAsync(string clientPath, string wzKey, string token);
        bool ValidatePreLaunch(string? clientPath, string? token);
        string GetPreLaunchValidationMessage(string? clientPath, string? token);
    }

    public class LaunchService : ILaunchService
    {
        private readonly ILoggingService _logger;
        private readonly IClientLocatorService _clientLocator;
        private readonly IConfigurationService _configService;

        public LaunchService(
            ILoggingService logger,
            IClientLocatorService clientLocator,
            IConfigurationService configService)
        {
            _logger = logger;
            _clientLocator = clientLocator;
            _configService = configService;
        }

        public async Task<(bool Success, string Message, int? ProcessId)> LaunchGameAsync(string clientPath, string wzKey, string token)
        {
            try
            {
                _logger.Log("Starting game launch sequence", "Launch");

                // Validate token
                if (string.IsNullOrEmpty(token))
                {
                    var message = "Authentication token is missing";
                    _logger.LogError(message);
                    return (false, message, null);
                }

                // Validate client path
                var validation = _clientLocator.ValidateClientPath(clientPath);
                if (!validation.IsValid)
                {
                    var message = validation.GetStatusMessage();
                    _logger.LogError($"Client validation failed: {message}");
                    return (false, message, null);
                }

                // Get full executable path
                var exePath = Path.Combine(clientPath, "MapleStory.exe");
                if (!File.Exists(exePath))
                {
                    var message = $"MapleStory.exe not found at {exePath}";
                    _logger.LogError(message);
                    return (false, message, null);
                }

                _logger.Log($"Client path validated: {clientPath}", "Launch");

                // Start the game process
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    WorkingDirectory = clientPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = false
                };

                // Set environment variables for the game process
                processStartInfo.EnvironmentVariables["SWORDIE_TOKEN"] = token;
                if (!string.IsNullOrEmpty(wzKey))
                {
                    processStartInfo.EnvironmentVariables["SWORDIE_KEY"] = wzKey;
                }

                _logger.Log("Launching MapleStory process", "Launch");
                var process = Process.Start(processStartInfo);

                if (process == null)
                {
                    var message = "Failed to start game process";
                    _logger.LogError(message);
                    return (false, message, null);
                }

                var entry = new LaunchLogEntry
                {
                    EventType = "Launch",
                    Message = "Game launched successfully",
                    ProcessId = process.Id
                };
                _logger.LogSuccess(entry);

                // Allow some time for DLL injection (simulated here, actual injection would be done by game)
                await Task.Delay(1000);

                return (true, "Game launched successfully", process.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Game launch failed", ex);
                return (false, $"Launch error: {ex.Message}", null);
            }
        }

        public bool ValidatePreLaunch(string? clientPath, string? token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            if (string.IsNullOrEmpty(clientPath))
                return false;

            var validation = _clientLocator.ValidateClientPath(clientPath);
            return validation.IsValid;
        }

        public string GetPreLaunchValidationMessage(string? clientPath, string? token)
        {
            if (string.IsNullOrEmpty(token))
                return "Not authenticated. Please login first.";

            if (string.IsNullOrEmpty(clientPath))
                return "MapleStory client not found. Configure in Settings.";

            var validation = _clientLocator.ValidateClientPath(clientPath);
            if (!validation.IsValid)
                return validation.GetStatusMessage();

            return "Ready to launch";
        }
    }
}
