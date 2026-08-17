using SpohnStory.Models;

namespace SpohnStory.Services
{
    public interface ILoggingService
    {
        void Log(string message, string? eventType = null);
        void LogError(string message, Exception? ex = null);
        void LogSuccess(LaunchLogEntry entry);
        void LogFailure(LaunchLogEntry entry);
        List<LaunchLogEntry> GetRecentLogs(int count = 50);
        string? GetLastError();
    }

    public class LoggingService : ILoggingService
    {
        private readonly string _logsPath;
        private readonly object _lockObj = new();
        private string? _lastError;

        public LoggingService()
        {
            _logsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory,
                "logs", 
                "launcher.log");

            var logsDir = Path.GetDirectoryName(_logsPath);
            if (!string.IsNullOrEmpty(logsDir) && !Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }
        }

        public void Log(string message, string? eventType = null)
        {
            var entry = new LaunchLogEntry
            {
                Timestamp = DateTime.Now,
                EventType = eventType ?? "Info",
                Message = message,
                Success = true
            };

            WriteLogEntry(entry);
        }

        public void LogError(string message, Exception? ex = null)
        {
            _lastError = message;
            var errorDetails = ex?.ToString() ?? string.Empty;
            var entry = new LaunchLogEntry
            {
                Timestamp = DateTime.Now,
                EventType = "Error",
                Message = message,
                ErrorDetails = errorDetails,
                Success = false
            };

            WriteLogEntry(entry);
        }

        public void LogSuccess(LaunchLogEntry entry)
        {
            entry.Success = true;
            entry.Timestamp = DateTime.Now;
            entry.EventType = entry.EventType ?? "Success";
            WriteLogEntry(entry);
        }

        public void LogFailure(LaunchLogEntry entry)
        {
            entry.Success = false;
            entry.Timestamp = DateTime.Now;
            entry.EventType = entry.EventType ?? "Failure";
            if (!string.IsNullOrEmpty(entry.ErrorDetails))
                _lastError = entry.ErrorDetails;
            WriteLogEntry(entry);
        }

        public List<LaunchLogEntry> GetRecentLogs(int count = 50)
        {
            var entries = new List<LaunchLogEntry>();

            try
            {
                if (!File.Exists(_logsPath))
                    return entries;

                var lines = File.ReadAllLines(_logsPath);
                var recentLines = lines.Skip(Math.Max(0, lines.Length - count)).ToList();

                foreach (var line in recentLines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var entry = ParseLogLine(line);
                    if (entry != null)
                        entries.Add(entry);
                }
            }
            catch
            {
                // Silent fail for log reading
            }

            return entries;
        }

        public string? GetLastError()
        {
            return _lastError;
        }

        private void WriteLogEntry(LaunchLogEntry entry)
        {
            lock (_lockObj)
            {
                try
                {
                    var line = entry.ToString();
                    File.AppendAllText(_logsPath, line + Environment.NewLine);
                }
                catch
                {
                    // Silent fail for file I/O
                }
            }
        }

        private LaunchLogEntry? ParseLogLine(string line)
        {
            try
            {
                // Simple line format: [timestamp] eventtype: message | details
                if (!line.Contains('[') || !line.Contains(']'))
                    return null;

                // This is a simplified parser - full parsing would handle all log format variations
                return new LaunchLogEntry
                {
                    Timestamp = DateTime.Now,
                    Message = line
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
