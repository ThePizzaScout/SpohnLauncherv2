namespace SpohnStory.Models
{
    public class LauncherConfiguration
    {
        public string? ApiUrl { get; set; } = "http://192.168.1.50:3000";
        public string? ClientPath { get; set; }
        public bool AutoDetectClient { get; set; } = true;
        public string Theme { get; set; } = "Dark";
        public string? AuthToken { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool SaveCredentials { get; set; } = false;
        public string? SavedUsername { get; set; }
        public string? SavedPassword { get; set; }
    }
}
