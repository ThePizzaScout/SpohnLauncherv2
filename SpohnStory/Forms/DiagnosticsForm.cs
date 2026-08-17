using SpohnStory.Services;
using SpohnStory.Configuration;
using SpohnStory.Models;

namespace SpohnStory.Forms
{
    public partial class DiagnosticsForm : Form
    {
        private readonly IConfigurationService _configService;
        private readonly IClientLocatorService _clientLocator;
        private readonly ILoggingService _logger;
        private readonly IAuthenticationService _authService;

        private TextBox? diagnosticsTextBox;
        private Button? copyButton;
        private Button? refreshButton;
        private Button? closeButton;

        public DiagnosticsForm(
            IConfigurationService configService,
            IClientLocatorService clientLocator,
            ILoggingService logger,
            IAuthenticationService authService)
        {
            _configService = configService;
            _clientLocator = clientLocator;
            _logger = logger;
            _authService = authService;

            CreateUI();
            ApplyDarkTheme();
            RefreshDiagnostics();
        }

        private void CreateUI()
        {
            Text = "SpohnStory Diagnostics";
            ClientSize = new Size(800, 600);
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = true;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            // Title
            var titleLabel = new Label
            {
                Text = "System Diagnostics",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(760, 35),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(titleLabel);

            // Diagnostics Text Box
            diagnosticsTextBox = new TextBox
            {
                Location = new Point(20, 65),
                Size = new Size(760, 440),
                Font = new Font("Consolas", 9),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.FromArgb(0, 255, 0),
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false
            };
            Controls.Add(diagnosticsTextBox);

            // Copy Button
            copyButton = new Button
            {
                Text = "📋 Copy Diagnostics",
                Location = new Point(20, 520),
                Size = new Size(250, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            copyButton.FlatAppearance.BorderSize = 0;
            copyButton.Click += CopyButton_Click;
            Controls.Add(copyButton);

            // Refresh Button
            refreshButton = new Button
            {
                Text = "🔄 Refresh",
                Location = new Point(290, 520),
                Size = new Size(250, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.Click += RefreshButton_Click;
            Controls.Add(refreshButton);

            // Close Button
            closeButton = new Button
            {
                Text = "Close",
                Location = new Point(560, 520),
                Size = new Size(220, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            closeButton.FlatAppearance.BorderSize = 0;
            Controls.Add(closeButton);
        }

        private void ApplyDarkTheme()
        {
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.White;
        }

        private void RefreshDiagnostics()
        {
            if (diagnosticsTextBox == null)
                return;

            var diagnostics = GatherDiagnosticInfo();
            diagnosticsTextBox.Text = diagnostics;
            _logger.Log("Diagnostics refreshed", "Diagnostics");
        }

        private string GatherDiagnosticInfo()
        {
            var lines = new List<string>();

            lines.Add("=== SpohnStory Launcher Diagnostics ===");
            lines.Add($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            lines.Add(string.Empty);

            // Application Info
            lines.Add("--- Application Info ---");
            lines.Add($"Application Path: {AppDomain.CurrentDomain.BaseDirectory}");
            lines.Add($"Configuration Path: {_configService.GetConfigurationPath()}");
            lines.Add(string.Empty);

            // Configuration
            lines.Add("--- Configuration ---");
            var config = _configService.GetConfiguration();
            lines.Add($"API URL: {config.ApiUrl}");
            lines.Add($"Client Path: {config.ClientPath ?? "Not configured"}");
            lines.Add($"Auto-detect Enabled: {config.AutoDetectClient}");
            lines.Add($"Theme: {config.Theme}");
            lines.Add(string.Empty);

            // Authentication
            lines.Add("--- Authentication ---");
            lines.Add($"Token Present: {_authService.HasValidToken()}");
            lines.Add($"Last Login: {config.LastLogin?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never"}");
            lines.Add(string.Empty);

            // Client Validation
            lines.Add("--- Client Validation ---");
            var validation = _clientLocator.ValidateClientPath(config.ClientPath);
            lines.Add($"Client Path Valid: {validation.IsValid}");
            lines.Add($"Client Path: {validation.ClientPath ?? "None"}");
            lines.Add($"MapleStory.exe Exists: {validation.MapleStoryExeExists}");
            lines.Add($"Swordie.dll Exists: {validation.SwordieDllExists}");
            lines.Add($"nmcogame64.dll Exists: {validation.NmCogame64DllExists}");
            lines.Add($"nmconew64.dll Exists: {validation.NmConew64DllExists}");
            if (!validation.IsValid && !string.IsNullOrEmpty(validation.ErrorMessage))
                lines.Add($"Error: {validation.ErrorMessage}");
            lines.Add(string.Empty);

            // Common Steam Locations
            lines.Add("--- Common Steam Locations ---");
            var commonLocations = _clientLocator.GetCommonSteamLocations();
            foreach (var location in commonLocations.Take(5))
            {
                var exists = Directory.Exists(location);
                lines.Add($"{(exists ? "✓" : "✗")} {location}");
            }
            lines.Add(string.Empty);

            // Detected Steam Libraries
            lines.Add("--- Steam Libraries ---");
            var steamLibraries = _clientLocator.GetSteamLibraryFolders();
            if (steamLibraries.Count > 0)
            {
                foreach (var library in steamLibraries.Take(5))
                {
                    var exists = Directory.Exists(library);
                    lines.Add($"{(exists ? "✓" : "✗")} {library}");
                }
            }
            else
            {
                lines.Add("No Steam libraries detected");
            }
            lines.Add(string.Empty);

            // Recent Logs
            lines.Add("--- Recent Logs ---");
            var recentLogs = _logger.GetRecentLogs(10);
            if (recentLogs.Count > 0)
            {
                foreach (var log in recentLogs)
                {
                    lines.Add(log.ToString());
                }
            }
            else
            {
                lines.Add("No recent logs");
            }

            if (!string.IsNullOrEmpty(_logger.GetLastError()))
            {
                lines.Add(string.Empty);
                lines.Add("--- Last Error ---");
                lines.Add(_logger.GetLastError());
            }

            lines.Add(string.Empty);
            lines.Add("=== End Diagnostics ===");

            return string.Join(Environment.NewLine, lines);
        }

        private void CopyButton_Click(object? sender, EventArgs e)
        {
            if (diagnosticsTextBox == null)
                return;

            try
            {
                Clipboard.SetText(diagnosticsTextBox.Text);
                _logger.Log("Diagnostics copied to clipboard", "Diagnostics");
                MessageBox.Show("Diagnostics copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to copy diagnostics", ex);
                MessageBox.Show($"Failed to copy: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object? sender, EventArgs e)
        {
            RefreshDiagnostics();
            MessageBox.Show("Diagnostics refreshed", "Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
