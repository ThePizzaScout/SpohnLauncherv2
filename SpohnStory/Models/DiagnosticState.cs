namespace SpohnStory.Models
{
    public class DiagnosticState
    {
        public string? ApiUrl { get; set; }
        public string? ClientPath { get; set; }
        public bool MapleStoryExeExists { get; set; }
        public bool SwordieDllExists { get; set; }
        public bool NmCogame64DllExists { get; set; }
        public bool NmConew64DllExists { get; set; }
        public bool TokenPresent { get; set; }
        public string? LastLoginResult { get; set; }
        public string? LastLaunchResult { get; set; }
        public string? LastInjectionResult { get; set; }
        public string? LastError { get; set; }
    }
}
