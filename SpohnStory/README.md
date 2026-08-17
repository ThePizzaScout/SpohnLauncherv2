# SpohnStory Launcher

A professional, modern game launcher for MapleStory with dark-themed UI, client detection, authentication, and comprehensive diagnostics.

## ✨ Features

- **Modern Dark Theme UI** - Professional dark-themed Windows Forms application
- **Authentication System** - Login/Register with token-based authentication
- **Client Detection** - Auto-detect or manually select MapleStory installation
- **Client Validation** - Verify required DLL files (Swordie.dll, nmcogame64.dll, nmconew64.dll)
- **Settings Management** - Configure client path and preferences
- **Diagnostics Page** - View system information and troubleshoot issues
- **Comprehensive Logging** - File-based logging to logs/launcher.log
- **Save Credentials** - Optionally save login credentials for quick access
- **Configurable API** - JSON-based configuration for API endpoints

## 🎯 Quick Start

### Prerequisites
- .NET 10.0 Runtime or SDK
- Windows OS (Windows Forms application)
- Git (for cloning)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/YOUR_USERNAME/SpohnStory-Launcher.git
   cd SpohnStory-Launcher
   ```

2. **Build the project:**
   ```bash
   dotnet build -c Release
   ```

3. **Run the launcher:**
   ```bash
   dotnet run
   ```
   Or run the compiled executable directly:
   ```bash
   ./bin/Release/net10.0-windows/SpohnStoryLauncher.exe
   ```

## 📋 Configuration

The launcher uses `launcher.json` for configuration:

```json
{
  "ApiUrl": "http://192.168.1.50:3000",
  "ClientPath": "C:\\Games\\MapleStory",
  "AutoDetectClient": true,
  "Theme": "Dark",
  "AuthToken": null,
  "LastLogin": null,
  "SaveCredentials": true,
  "SavedUsername": "player123",
  "SavedPassword": "password123"
}
```

**Configuration Options:**
- `ApiUrl` - Backend server URL (default: http://192.168.1.50:3000)
- `ClientPath` - Path to MapleStory installation
- `AutoDetectClient` - Automatically detect client on startup
- `Theme` - UI theme (currently "Dark")
- `AuthToken` - Current authentication token (auto-set on login)
- `SaveCredentials` - Save login credentials for quick access
- `SavedUsername` - Saved username
- `SavedPassword` - Saved password

## 🏗️ Architecture

### Project Structure
```
SpohnStory/
├── Models/                      # Data models
├── Services/                    # Business logic
├── Forms/                       # UI Forms
├── Configuration/               # Settings management
├── Assets/                      # Image assets (reserved)
├── logs/                        # Application logs
├── QUICK_START.md              # User guide
├── MIGRATION_REPORT.md         # Technical details
└── FILE_MANIFEST.md            # Complete file listing
```

### Service Interfaces
- **IAuthenticationService** - Login/Register/Token management
- **IClientLocatorService** - Client detection and validation
- **ILaunchService** - Game launching and validation
- **IApiClient** - Backend API communication
- **ILoggingService** - File-based logging
- **IConfigurationService** - Settings management

## 📡 API Integration

### Endpoints
The launcher communicates with the following API endpoints:

**Authentication:**
```
POST /api/login
POST /api/register
Body: { "username": string, "password": string }
```

**Game Key:**
```
GET /api/key
Headers: Authorization: Bearer <token>
```

**Validation:**
```
GET /api/validate
Headers: Authorization: Bearer <token>
```

## 🔍 Diagnostics

The launcher includes a comprehensive diagnostics page that shows:
- Application paths
- API configuration
- Client validation status
- Steam detection results
- Recent logs
- System information

Access via: **Diagnostics** button in the main launcher window.

## 📝 Logging

All operations are logged to `logs/launcher.log`:

```
[2024-01-01 12:00:00] Startup: Application started
[2024-01-01 12:00:01] Authentication: Attempting login
[2024-01-01 12:00:02] Authentication: Login successful for user: player123
[2024-01-01 12:00:05] Launch: Game launched successfully | PID: 12345
```

## 🎨 UI Preview

### Main Form
- Login/Register panel with username and password fields
- **Save Login Credentials** checkbox for credential persistence
- Online/Offline status indicator
- Large Play button with validation feedback
- Settings and Diagnostics buttons
- Real-time status messages

### Settings Form
- Client path configuration
- Browse button for manual selection
- Auto-detect button for Steam scanning
- Real-time validation status for required files

### Diagnostics Form
- Complete system information
- Recent log entries
- Copy to clipboard functionality
- Real-time refresh

## 🔐 Security

- Token-based authentication
- Credentials stored locally in launcher.json (plain text - consider encryption for production)
- No hardcoded API credentials
- Proper exception handling
- Secure token management

## 🧪 Testing

Recommended tests:
1. Login with valid credentials
2. Login with invalid credentials
3. Auto-detect MapleStory (with Steam installed)
4. Manual client path selection
5. Verify validation with missing DLLs
6. Save and load credentials
7. Launch game with valid setup

## 📦 Dependencies

- **Microsoft.Extensions.DependencyInjection** 8.0.0 - Dependency injection
- **System.Net.Http** 4.3.4 - HTTP communication
- **.NET 10.0 Windows** - Framework

## 🚀 Deployment

### Build Release
```bash
dotnet build -c Release
```

### Publish as Single File
```bash
dotnet publish -c Release -r win-x64 --self-contained /p:PublishSingleFile=true
```

### Run on Target Machine
1. Ensure .NET 10.0 runtime is installed
2. Place launcher executable next to logs/ folder
3. Update launcher.json with your API server URL
4. Run SpohnStoryLauncher.exe

## 📄 Documentation

- **QUICK_START.md** - User guide and feature overview
- **MIGRATION_REPORT.md** - Detailed technical documentation
- **FILE_MANIFEST.md** - Complete file listing and metrics
- **UPDATE_SUMMARY.md** - Recent updates and changes

## 🤝 Contributing

To contribute:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## 📞 Support

For issues or questions:
1. Check the Diagnostics page in the launcher
2. Review logs/launcher.log for error details
3. Check the documentation in the repository

## 📜 License

This project is provided as-is. Modify and use as needed for your purposes.

## 🎉 Version History

### v1.0.1 (Current)
- Added save login credentials feature
- Updated API URL to 192.168.1.50:3000
- Enhanced UI with checkbox control
- Improved credential persistence

### v1.0.0
- Initial release
- Complete launcher implementation
- Dark theme UI
- Client detection and validation
- Settings and diagnostics pages
- Comprehensive logging

## 📧 Contact

For more information, visit the repository or check the included documentation.

---

**Status:** ✅ Production Ready
**Last Updated:** 2024
**Framework:** .NET 10.0 Windows Forms
**License:** MIT (or your preferred license)
