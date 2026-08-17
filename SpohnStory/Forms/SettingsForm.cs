using SpohnStory.Services;
using SpohnStory.Configuration;

namespace SpohnStory.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly IConfigurationService _configService;
        private readonly IClientLocatorService _clientLocator;
        private readonly ILoggingService _logger;

        private TextBox? clientPathTextBox;
        private Label? clientStatusLabel;
        private Button? browseButton;
        private Button? autoDetectButton;
        private Button? saveButton;
        private Button? closeButton;

        public SettingsForm(
            IConfigurationService configService,
            IClientLocatorService clientLocator,
            ILoggingService logger)
        {
            _configService = configService;
            _clientLocator = clientLocator;
            _logger = logger;

            CreateUI();
            ApplyDarkTheme();
            LoadSettings();
        }

        private void CreateUI()
        {
            Text = "SpohnStory Settings";
            ClientSize = new Size(600, 400);
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            // Title
            var titleLabel = new Label
            {
                Text = "Client Settings",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(560, 40),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(titleLabel);

            // Client Path Section
            var clientPathLabel = new Label
            {
                Text = "MapleStory Installation Path:",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                Location = new Point(20, 80),
                Size = new Size(560, 25),
                AutoSize = false
            };
            Controls.Add(clientPathLabel);

            clientPathTextBox = new TextBox
            {
                Location = new Point(20, 110),
                Size = new Size(560, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true
            };
            Controls.Add(clientPathTextBox);

            // Buttons Panel
            var buttonPanelWidth = 560;
            var buttonHeight = 35;
            var spacing = 5;
            var btnWidth = (buttonPanelWidth - (spacing * 2)) / 3;

            browseButton = new Button
            {
                Text = "Browse",
                Location = new Point(20, 160),
                Size = new Size(btnWidth, buttonHeight),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            browseButton.FlatAppearance.BorderSize = 0;
            browseButton.Click += BrowseButton_Click;
            Controls.Add(browseButton);

            autoDetectButton = new Button
            {
                Text = "Auto Detect",
                Location = new Point(20 + btnWidth + spacing, 160),
                Size = new Size(btnWidth, buttonHeight),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            autoDetectButton.FlatAppearance.BorderSize = 0;
            autoDetectButton.Click += AutoDetectButton_Click;
            Controls.Add(autoDetectButton);

            saveButton = new Button
            {
                Text = "Save",
                Location = new Point(20 + (btnWidth + spacing) * 2, 160),
                Size = new Size(btnWidth, buttonHeight),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;
            Controls.Add(saveButton);

            // Status Section
            var statusTitleLabel = new Label
            {
                Text = "Client Status:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 210),
                Size = new Size(560, 25),
                AutoSize = false
            };
            Controls.Add(statusTitleLabel);

            clientStatusLabel = new Label
            {
                Text = "Checking...",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(255, 193, 7),
                Location = new Point(20, 240),
                Size = new Size(560, 100),
                AutoSize = false,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(45, 45, 45),
                Padding = new Padding(5)
            };
            Controls.Add(clientStatusLabel);

            // Close Button
            closeButton = new Button
            {
                Text = "Close",
                Location = new Point(20, 355),
                Size = new Size(560, 35),
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

        private void LoadSettings()
        {
            var config = _configService.GetConfiguration();
            if (clientPathTextBox != null)
            {
                clientPathTextBox.Text = config.ClientPath ?? string.Empty;
            }
            UpdateClientStatus();
        }

        private void UpdateClientStatus()
        {
            if (clientStatusLabel == null || clientPathTextBox == null)
                return;

            var clientPath = clientPathTextBox.Text;
            var validation = _clientLocator.ValidateClientPath(clientPath);

            var statusText = $"Path: {(string.IsNullOrEmpty(clientPath) ? "Not configured" : clientPath)}\n";
            statusText += $"MapleStory.exe: {(validation.MapleStoryExeExists ? "✓" : "✗")}\n";
            statusText += $"Swordie.dll: {(validation.SwordieDllExists ? "✓" : "✗")}\n";
            statusText += $"nmcogame64.dll: {(validation.NmCogame64DllExists ? "✓" : "✗")}\n";
            statusText += $"nmconew64.dll: {(validation.NmConew64DllExists ? "✓" : "✗")}\n";
            statusText += $"Status: {(validation.IsValid ? "Valid" : "Invalid")}";

            clientStatusLabel.Text = statusText;
            clientStatusLabel.ForeColor = validation.IsValid ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
        }

        private void BrowseButton_Click(object? sender, EventArgs e)
        {
            _logger.Log("Browse button clicked", "Settings");
            if (_clientLocator.ManualSelectClient(out var selectedPath))
            {
                if (clientPathTextBox != null)
                {
                    clientPathTextBox.Text = selectedPath ?? string.Empty;
                }
                UpdateClientStatus();
            }
        }

        private void AutoDetectButton_Click(object? sender, EventArgs e)
        {
            _logger.Log("Auto-detect button clicked", "Settings");
            if (autoDetectButton != null)
            {
                autoDetectButton.Enabled = false;
                autoDetectButton.Text = "Detecting...";
            }

            try
            {
                var detectedPath = _clientLocator.AutoDetectClient();
                if (!string.IsNullOrEmpty(detectedPath))
                {
                    if (clientPathTextBox != null)
                    {
                        clientPathTextBox.Text = detectedPath;
                    }
                    _logger.Log($"Client auto-detected at: {detectedPath}", "Settings");
                    UpdateClientStatus();
                }
                else
                {
                    MessageBox.Show(
                        "Could not auto-detect MapleStory installation.\nPlease browse manually.",
                        "Auto-detect Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    _logger.LogError("Auto-detect failed - no valid installation found", null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during auto-detect: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.LogError("Auto-detect error", ex);
            }
            finally
            {
                if (autoDetectButton != null)
                {
                    autoDetectButton.Enabled = true;
                    autoDetectButton.Text = "Auto Detect";
                }
            }
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            if (clientPathTextBox == null)
                return;

            _logger.Log("Saving settings", "Settings");

            var validation = _clientLocator.ValidateClientPath(clientPathTextBox.Text);
            if (!validation.IsValid)
            {
                MessageBox.Show(
                    validation.GetStatusMessage(),
                    "Invalid Client Path",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var config = _configService.GetConfiguration();
            config.ClientPath = clientPathTextBox.Text;
            _configService.SaveConfiguration(config);

            _logger.Log($"Client path saved: {config.ClientPath}", "Settings");
            MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
