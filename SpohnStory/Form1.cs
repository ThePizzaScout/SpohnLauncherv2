using SpohnStory.Services;
using SpohnStory.Models;
using SpohnStory.Forms;
using SpohnStory.Configuration;

namespace SpohnStory
{
    public partial class Form1 : Form
    {
        private readonly ILoggingService _logger;
        private readonly IConfigurationService _configService;
        private readonly IClientLocatorService _clientLocator;
        private readonly IAuthenticationService _authService;
        private readonly ILaunchService _launchService;
        private readonly IApiClient _apiClient;

        private Label? titleLabel;
        private Label? statusLabel;
        private Panel? loginPanel;
        private TextBox? usernameTextBox;
        private TextBox? passwordTextBox;
        private CheckBox? saveLoginCheckBox;
        private Button? loginButton;
        private Button? registerButton;
        private Button? playButton;
        private Button? settingsButton;
        private Button? diagnosticsButton;
        private Label? messageLabel;
        private Label? statusIndicatorLabel;

        public Form1(
            ILoggingService logger,
            IConfigurationService configService,
            IClientLocatorService clientLocator,
            IAuthenticationService authService,
            ILaunchService launchService,
            IApiClient apiClient)
        {
            _logger = logger;
            _configService = configService;
            _clientLocator = clientLocator;
            _authService = authService;
            _launchService = launchService;
            _apiClient = apiClient;

            CreateUI();
            ApplyDarkTheme();
            CheckAuthenticationStatus();
            _logger.Log("Application started", "Startup");
        }

        private void CreateUI()
        {
            Text = "SpohnStory Launcher";
            ClientSize = new Size(600, 550);
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            // Title
            titleLabel = new Label
            {
                Text = "SpohnStory",
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(560, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(titleLabel);

            // Status Indicator
            statusIndicatorLabel = new Label
            {
                Text = "● Offline",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(220, 53, 69),
                Location = new Point(20, 90),
                Size = new Size(200, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(statusIndicatorLabel);

            // Login Panel
            loginPanel = new Panel
            {
                Location = new Point(20, 130),
                Size = new Size(560, 280),
                BackColor = Color.FromArgb(45, 45, 45),
                BorderStyle = BorderStyle.FixedSingle
            };

            var usernameLabel = new Label
            {
                Text = "Username:",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                Location = new Point(10, 15),
                Size = new Size(540, 25),
                AutoSize = false
            };
            loginPanel.Controls.Add(usernameLabel);

            usernameTextBox = new TextBox
            {
                Location = new Point(10, 40),
                Size = new Size(540, 35),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            usernameTextBox.ForeColorChanged += (s, e) => usernameTextBox.ForeColor = Color.White;
            loginPanel.Controls.Add(usernameTextBox);

            var passwordLabel = new Label
            {
                Text = "Password:",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                Location = new Point(10, 85),
                Size = new Size(540, 25),
                AutoSize = false
            };
            loginPanel.Controls.Add(passwordLabel);

            passwordTextBox = new TextBox
            {
                Location = new Point(10, 110),
                Size = new Size(540, 35),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true
            };
            loginPanel.Controls.Add(passwordTextBox);

            // Save Login Checkbox
            saveLoginCheckBox = new CheckBox
            {
                Text = "Save Login Credentials",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                Location = new Point(10, 155),
                Size = new Size(540, 25),
                BackColor = Color.FromArgb(45, 45, 45),
                AutoSize = false,
                Padding = new Padding(5, 3, 0, 0)
            };
            loginPanel.Controls.Add(saveLoginCheckBox);

            loginButton = new Button
            {
                Text = "Login",
                Location = new Point(10, 190),
                Size = new Size(260, 40),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.Click += LoginButton_Click;
            loginPanel.Controls.Add(loginButton);

            registerButton = new Button
            {
                Text = "Register",
                Location = new Point(290, 190),
                Size = new Size(260, 40),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            registerButton.FlatAppearance.BorderSize = 0;
            registerButton.Click += RegisterButton_Click;
            loginPanel.Controls.Add(registerButton);

            Controls.Add(loginPanel);

            // Play Button
            playButton = new Button
            {
                Text = "▶ PLAY",
                Location = new Point(20, 400),
                Size = new Size(560, 50),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            playButton.FlatAppearance.BorderSize = 0;
            playButton.Click += PlayButton_Click;
            Controls.Add(playButton);

            // Message Label
            messageLabel = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(255, 193, 7),
                Location = new Point(20, 460),
                Size = new Size(560, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(messageLabel);

            // Settings Button
            settingsButton = new Button
            {
                Text = "⚙ Settings",
                Location = new Point(20, 500),
                Size = new Size(270, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            settingsButton.FlatAppearance.BorderSize = 0;
            settingsButton.Click += SettingsButton_Click;
            Controls.Add(settingsButton);

            // Diagnostics Button
            diagnosticsButton = new Button
            {
                Text = "📋 Diagnostics",
                Location = new Point(310, 500),
                Size = new Size(270, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            diagnosticsButton.FlatAppearance.BorderSize = 0;
            diagnosticsButton.Click += DiagnosticsButton_Click;
            Controls.Add(diagnosticsButton);
        }

        private void ApplyDarkTheme()
        {
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.White;
        }

        private void CheckAuthenticationStatus()
        {
            var config = _configService.GetConfiguration();
            var hasToken = _authService.HasValidToken();

            // Load saved credentials if available
            if (config.SaveCredentials && !string.IsNullOrEmpty(config.SavedUsername))
            {
                if (usernameTextBox != null)
                    usernameTextBox.Text = config.SavedUsername;
                if (passwordTextBox != null)
                    passwordTextBox.Text = config.SavedPassword ?? string.Empty;
                if (saveLoginCheckBox != null)
                    saveLoginCheckBox.Checked = true;
            }

            if (loginPanel != null)
                loginPanel.Visible = !hasToken;
            if (playButton != null)
                playButton.Enabled = hasToken;
            if (statusIndicatorLabel != null)
            {
                if (hasToken)
                {
                    statusIndicatorLabel.Text = "● Online";
                    statusIndicatorLabel.ForeColor = Color.FromArgb(40, 167, 69);
                }
                else
                {
                    statusIndicatorLabel.Text = "● Offline";
                    statusIndicatorLabel.ForeColor = Color.FromArgb(220, 53, 69);
                }
            }

            UpdatePlayButtonStatus();
        }

        private void UpdatePlayButtonStatus()
        {
            if (playButton != null && messageLabel != null)
            {
                var config = _configService.GetConfiguration();
                var validationMessage = _launchService.GetPreLaunchValidationMessage(config.ClientPath, _authService.GetTokenAsync());
                messageLabel.Text = validationMessage;
                playButton.Enabled = _launchService.ValidatePreLaunch(config.ClientPath, _authService.GetTokenAsync());
                messageLabel.ForeColor = playButton.Enabled ? Color.FromArgb(40, 167, 69) : Color.FromArgb(255, 193, 7);
            }
        }

        private async void LoginButton_Click(object? sender, EventArgs e)
        {
            if (usernameTextBox == null || passwordTextBox == null || loginButton == null)
                return;

            loginButton.Enabled = false;
            loginButton.Text = "Logging in...";

            try
            {
                var response = await _authService.LoginAsync(usernameTextBox.Text, passwordTextBox.Text);
                if (response?.Success == true)
                {
                    // Save credentials if checkbox is checked
                    if (saveLoginCheckBox?.Checked == true)
                    {
                        var config = _configService.GetConfiguration();
                        config.SaveCredentials = true;
                        config.SavedUsername = usernameTextBox.Text;
                        config.SavedPassword = passwordTextBox.Text;
                        _configService.SaveConfiguration(config);
                        _logger.Log("Login credentials saved", "Authentication");
                    }
                    else
                    {
                        // Clear saved credentials if unchecked
                        var config = _configService.GetConfiguration();
                        config.SaveCredentials = false;
                        config.SavedUsername = null;
                        config.SavedPassword = null;
                        _configService.SaveConfiguration(config);
                    }

                    _logger.Log("Login successful", "Authentication");
                    CheckAuthenticationStatus();
                    if (messageLabel != null)
                        messageLabel.Text = "Login successful!";
                }
                else
                {
                    _logger.LogError($"Login failed: {response?.Message}");
                    if (messageLabel != null)
                        messageLabel.Text = $"Login failed: {response?.Message ?? "Unknown error"}";
                    if (messageLabel != null)
                        messageLabel.ForeColor = Color.FromArgb(220, 53, 69);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Login error", ex);
                if (messageLabel != null)
                    messageLabel.Text = $"Error: {ex.Message}";
            }
            finally
            {
                loginButton.Enabled = true;
                loginButton.Text = "Login";
            }
        }

        private async void RegisterButton_Click(object? sender, EventArgs e)
        {
            if (usernameTextBox == null || passwordTextBox == null || registerButton == null)
                return;

            registerButton.Enabled = false;
            registerButton.Text = "Registering...";

            try
            {
                var response = await _authService.RegisterAsync(usernameTextBox.Text, passwordTextBox.Text);
                if (response?.Success == true)
                {
                    _logger.Log("Registration successful", "Authentication");
                    if (messageLabel != null)
                        messageLabel.Text = "Registration successful! Please login.";
                }
                else
                {
                    _logger.LogError($"Registration failed: {response?.Message}");
                    if (messageLabel != null)
                        messageLabel.Text = $"Registration failed: {response?.Message ?? "Unknown error"}";
                    if (messageLabel != null)
                        messageLabel.ForeColor = Color.FromArgb(220, 53, 69);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Registration error", ex);
                if (messageLabel != null)
                    messageLabel.Text = $"Error: {ex.Message}";
            }
            finally
            {
                registerButton.Enabled = true;
                registerButton.Text = "Register";
            }
        }

        private async void PlayButton_Click(object? sender, EventArgs e)
        {
            if (playButton == null || messageLabel == null)
                return;

            playButton.Enabled = false;
            playButton.Text = "Launching...";

            try
            {
                var config = _configService.GetConfiguration();
                var token = _authService.GetTokenAsync();

                if (string.IsNullOrEmpty(token))
                {
                    messageLabel.Text = "Authentication required. Please login.";
                    messageLabel.ForeColor = Color.FromArgb(220, 53, 69);
                    return;
                }

                if (string.IsNullOrEmpty(config.ClientPath))
                {
                    messageLabel.Text = "Client path not configured. Go to Settings.";
                    messageLabel.ForeColor = Color.FromArgb(220, 53, 69);
                    return;
                }

                messageLabel.Text = "Requesting WZ key...";
                messageLabel.ForeColor = Color.White;

                var wzKey = await _apiClient.GetWzKeyAsync(token) ?? string.Empty;

                messageLabel.Text = "Launching game...";
                var launchResult = await _launchService.LaunchGameAsync(config.ClientPath, wzKey, token);

                if (launchResult.Success)
                {
                    messageLabel.Text = $"Game launched (PID: {launchResult.ProcessId})";
                    messageLabel.ForeColor = Color.FromArgb(40, 167, 69);
                }
                else
                {
                    messageLabel.Text = $"Launch failed: {launchResult.Message}";
                    messageLabel.ForeColor = Color.FromArgb(220, 53, 69);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Play button error", ex);
                messageLabel.Text = $"Error: {ex.Message}";
                messageLabel.ForeColor = Color.FromArgb(220, 53, 69);
            }
            finally
            {
                playButton.Enabled = true;
                playButton.Text = "▶ PLAY";
            }
        }

        private void SettingsButton_Click(object? sender, EventArgs e)
        {
            _logger.Log("Opening Settings form", "UI");
            var settingsForm = new SettingsForm(_configService, _clientLocator, _logger);
            if (settingsForm.ShowDialog(this) == DialogResult.OK)
            {
                UpdatePlayButtonStatus();
                _logger.Log("Settings updated", "UI");
            }
        }

        private void DiagnosticsButton_Click(object? sender, EventArgs e)
        {
            _logger.Log("Opening Diagnostics form", "UI");
            var diagnosticsForm = new DiagnosticsForm(_configService, _clientLocator, _logger, _authService);
            diagnosticsForm.ShowDialog(this);
        }
    }
}
