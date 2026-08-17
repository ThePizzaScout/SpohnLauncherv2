using SpohnStory.Models;

namespace SpohnStory.Services
{
    public interface IClientLocatorService
    {
        ClientValidationResult ValidateClientPath(string? clientPath);
        string? AutoDetectClient();
        bool ManualSelectClient(out string? selectedPath);
        List<string> GetCommonSteamLocations();
        List<string> GetSteamLibraryFolders();
    }

    public class ClientLocatorService : IClientLocatorService
    {
        private const string MapleStoryExe = "MapleStory.exe";
        private const string SwordieDll = "Swordie.dll";
        private const string NmCogame64Dll = "nmcogame64.dll";
        private const string NmConew64Dll = "nmconew64.dll";

        public ClientValidationResult ValidateClientPath(string? clientPath)
        {
            var result = new ClientValidationResult
            {
                ClientPath = clientPath
            };

            if (string.IsNullOrWhiteSpace(clientPath))
            {
                result.IsValid = false;
                result.ErrorMessage = "Client path not specified";
                return result;
            }

            // If it's a file path, get the directory
            var dirPath = clientPath;
            if (File.Exists(clientPath) && Path.GetFileName(clientPath).Equals(MapleStoryExe, StringComparison.OrdinalIgnoreCase))
            {
                dirPath = Path.GetDirectoryName(clientPath) ?? clientPath;
            }

            // Check if directory exists
            if (!Directory.Exists(dirPath))
            {
                result.IsValid = false;
                result.ErrorMessage = $"Directory not found: {dirPath}";
                return result;
            }

            // Check for required files
            result.MapleStoryExeExists = File.Exists(Path.Combine(dirPath, MapleStoryExe));
            result.SwordieDllExists = File.Exists(Path.Combine(dirPath, SwordieDll));
            result.NmCogame64DllExists = File.Exists(Path.Combine(dirPath, NmCogame64Dll));
            result.NmConew64DllExists = File.Exists(Path.Combine(dirPath, NmConew64Dll));

            result.IsValid = result.MapleStoryExeExists && 
                            result.SwordieDllExists && 
                            result.NmCogame64DllExists && 
                            result.NmConew64DllExists;

            result.ClientPath = dirPath;
            return result;
        }

        public string? AutoDetectClient()
        {
            var locations = new List<string>();
            locations.AddRange(GetCommonSteamLocations());
            locations.AddRange(GetSteamLibraryFolders());

            foreach (var location in locations)
            {
                if (!string.IsNullOrEmpty(location) && Directory.Exists(location))
                {
                    var validation = ValidateClientPath(location);
                    if (validation.IsValid)
                    {
                        return location;
                    }
                }
            }

            return null;
        }

        public bool ManualSelectClient(out string? selectedPath)
        {
            selectedPath = null;
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select MapleStory installation folder";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var validation = ValidateClientPath(dialog.SelectedPath);
                    if (validation.IsValid)
                    {
                        selectedPath = dialog.SelectedPath;
                        return true;
                    }
                }
            }
            return false;
        }

        public List<string> GetCommonSteamLocations()
        {
            var locations = new List<string>();
            var commonPaths = new[]
            {
                @"C:\Program Files (x86)\Steam\steamapps\common\MapleStory",
                @"C:\Program Files\Steam\steamapps\common\MapleStory",
                @"D:\SteamLibrary\steamapps\common\MapleStory",
                @"D:\Games\MapleStory",
                @"D:\MapleStory",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam", "steamapps", "common", "MapleStory"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Steam", "steamapps", "common", "MapleStory"),
            };

            foreach (var path in commonPaths)
            {
                if (!string.IsNullOrEmpty(path) && !locations.Contains(path, StringComparer.OrdinalIgnoreCase))
                {
                    locations.Add(path);
                }
            }

            return locations;
        }

        public List<string> GetSteamLibraryFolders()
        {
            var locations = new List<string>();
            var steamPath = GetSteamPath();

            if (string.IsNullOrEmpty(steamPath))
                return locations;

            try
            {
                var libraryFoldersPath = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
                if (File.Exists(libraryFoldersPath))
                {
                    var content = File.ReadAllText(libraryFoldersPath);
                    var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        if (line.Contains("path") && line.Contains("\""))
                        {
                            var parts = line.Split('"');
                            if (parts.Length >= 4)
                            {
                                var path = parts[3];
                                if (!string.IsNullOrEmpty(path))
                                {
                                    var mapleStoryPath = Path.Combine(path, "steamapps", "common", "MapleStory");
                                    if (!locations.Contains(mapleStoryPath, StringComparer.OrdinalIgnoreCase))
                                    {
                                        locations.Add(mapleStoryPath);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Silent fail for Steam library parsing
            }

            return locations;
        }

        private string? GetSteamPath()
        {
            try
            {
                var possiblePaths = new[]
                {
                    @"C:\Program Files (x86)\Steam",
                    @"C:\Program Files\Steam",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Steam"),
                };

                foreach (var path in possiblePaths)
                {
                    if (Directory.Exists(path))
                        return path;
                }
            }
            catch
            {
                // Silent fail
            }

            return null;
        }
    }
}
