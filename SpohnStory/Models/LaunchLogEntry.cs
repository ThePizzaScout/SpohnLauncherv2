namespace SpohnStory.Models
{
    public class LaunchLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string? EventType { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }
        public string? ErrorDetails { get; set; }
        public int? ProcessId { get; set; }

        public override string ToString()
        {
            var result = $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] {EventType}: {Message}";
            if (!Success && !string.IsNullOrEmpty(ErrorDetails))
                result += $" | Error: {ErrorDetails}";
            if (ProcessId.HasValue)
                result += $" | PID: {ProcessId}";
            return result;
        }
    }
}
