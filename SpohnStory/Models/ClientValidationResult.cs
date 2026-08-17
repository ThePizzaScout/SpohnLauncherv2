namespace SpohnStory.Models
{
    public class ClientValidationResult
    {
        public bool IsValid { get; set; }
        public string? ClientPath { get; set; }
        public bool MapleStoryExeExists { get; set; }
        public bool SwordieDllExists { get; set; }
        public bool NmCogame64DllExists { get; set; }
        public bool NmConew64DllExists { get; set; }
        public string? ErrorMessage { get; set; }

        public string GetStatusMessage()
        {
            if (!IsValid && !string.IsNullOrEmpty(ErrorMessage))
                return ErrorMessage;

            var missingFiles = new List<string>();
            if (!MapleStoryExeExists) missingFiles.Add("MapleStory.exe");
            if (!SwordieDllExists) missingFiles.Add("Swordie.dll");
            if (!NmCogame64DllExists) missingFiles.Add("nmcogame64.dll");
            if (!NmConew64DllExists) missingFiles.Add("nmconew64.dll");

            if (missingFiles.Count > 0)
                return $"Missing required files: {string.Join(", ", missingFiles)}";

            return IsValid ? "Client is valid" : "Client is invalid";
        }
    }
}
