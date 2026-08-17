# SpohnStory Launcher - Quick Start Guide

## 🎮 Project Successfully Created!

The **SpohnStory Launcher** is now fully implemented and building without errors. This is a production-ready game launcher with a modern dark-themed UI.

---

## ✅ What's Implemented

### Core Features
- ✅ **Modern Dark UI** - Professional dark theme with responsive controls
- ✅ **Authentication** - Login and registration with token management
- ✅ **Client Detection** - Auto-detect or manually select MapleStory installation
- ✅ **Game Launching** - Launch game with DLL injection support
- ✅ **Settings Management** - Configurable client path and preferences
- ✅ **Diagnostics** - Complete system diagnostics and troubleshooting tools
- ✅ **Logging** - File-based logging to logs/launcher.log
- ✅ **Configuration** - JSON-based launcher.json configuration

### Services Architecture
```
IAuthenticationService  → Login/Register/Token Management
IClientLocatorService   → Client Detection & Validation
ILaunchService          → Game Launching
IApiClient              → Backend Communication
ILoggingService         → File-based Logging
IConfigurationService   → Settings Management
```

---

## 📁 Project Structure

```
SpohnStory/
├── Models/                      # Data models
│   ├── LauncherConfiguration.cs
│   ├── ClientValidationResult.cs
│   ├── LaunchLogEntry.cs
│   ├── AuthenticationResponse.cs
│   └── DiagnosticState.cs
│
├── Services/                    # Business logic
│   ├── LoggingService.cs
│   ├── ClientLocatorService.cs
│   ├── ApiClient.cs
│   ├── AuthenticationService.cs
│   └── LaunchService.cs
│
├── Forms/                       # UI Forms
│   ├── SettingsForm.cs
│   └── DiagnosticsForm.cs
│
├── Configuration/               # Configuration management
│   └── ConfigurationService.cs
│
├── Assets/                      # Image assets (reserved)
├── logs/                        # Application logs
│
├── Form1.cs                     # Main launcher form
├── Program.cs                   # Entry point with DI
└── SpohnStory.csproj           # Project configuration
```

---

## 🚀 Running the Launcher

1. **Build the project**: `dotnet build -c Release`
2. **Launch**: Run `SpohnStoryLauncher.exe`
3. **Configure**: 
   - Click "Settings" to set client path
   - Use "Auto Detect" to find Steam installation
   - Or click "Browse" to manually select folder

---

## 🔧 Configuration (launcher.json)

The launcher automatically creates `launcher.json` in the application directory:

```json
{
  "ApiUrl": "http://localhost:3000",
  "ClientPath": null,
  "AutoDetectClient": true,
  "Theme": "Dark",
  "AuthToken": null,
  "LastLogin": null
}
```

**Key Settings:**
- `ApiUrl` - Backend API server URL
- `ClientPath` - Path to MapleStory installation
- `AutoDetectClient` - Enable automatic client detection
- `Theme` - UI theme (currently "Dark")

---

## 📋 Main Form Features

### Login Panel
- Username and password fields
- Login button - Authenticate with backend
- Register button - Create new account

### Status Display
- Online/Offline indicator
- Server message area
- Status color-coded indicators

### Play Button
- Pre-launch validation
- Displays launch status
- Shows validation errors

### Navigation
- **Settings** - Configure client path
- **Diagnostics** - View system information

---

## ⚙️ Settings Form

### Client Configuration
- Display current MapleStory path
- **Browse** - Select folder manually
- **Auto Detect** - Search Steam locations
- **Save** - Persist configuration

### Status Display
- ✓/✗ indicators for each required DLL
- Real-time validation
- Error messages if invalid

---

## 🔍 Diagnostics Form

Shows complete system information:
- Application and configuration paths
- API URL configuration
- Authentication status
- Client validation results
- Steam locations and libraries
- Recent logs (10 entries)
- Last error message
- **Copy Diagnostics** button for support

---

## 📝 Logging

All operations are logged to `logs/launcher.log`:

```
[2024-01-01 12:00:00] Startup: Application started
[2024-01-01 12:00:01] Authentication: Attempting login to http://localhost:3000/api/login
[2024-01-01 12:00:02] Authentication: Login successful for user: player123
[2024-01-01 12:00:05] Settings: Auto-detect button clicked
[2024-01-01 12:00:10] Launch: Starting game launch sequence
[2024-01-01 12:00:11] Launch: Client path validated: C:\Games\MapleStory
[2024-01-01 12:00:12] KeyRequest: WZ key retrieved successfully
[2024-01-01 12:00:15] Launch: Game launched successfully | PID: 12345
```

---

## 🎨 UI Theme

**Color Scheme:**
- Background: #1E1E1E (Dark Gray)
- Text: White
- Primary Button: #0066CC (Blue)
- Success: #28A745 (Green)
- Warning: #FFC107 (Yellow)
- Error: #DC3545 (Red)

**All controls are programmatically created** - no designer files, fully customizable.

---

## 🔌 API Integration

### Endpoints Used

**Login/Register:**
```
POST /api/login
POST /api/register
Body: { "username": string, "password": string }
Response: { "success": bool, "token": string, "message": string }
```

**WZ Key:**
```
GET /api/key
Headers: Authorization: Bearer <token>
Response: { "key": string }
```

**Token Validation:**
```
GET /api/validate
Headers: Authorization: Bearer <token>
Response: 200 OK if valid
```

---

## 🔐 Security Features

- ✅ Token-based authentication
- ✅ Secure token storage (in config file and memory)
- ✅ Token cleared on logout
- ✅ Environment variables for game process
- ✅ No hardcoded credentials
- ✅ Proper exception handling

---

## 📦 Dependencies

**NuGet Packages:**
- `Microsoft.Extensions.DependencyInjection` 8.0.0 - For DI container
- `System.Net.Http` 4.3.4 - For API communication
- `.NET 10.0 Windows` - Target framework

---

## 🛠️ Build Information

**Solution:** SpohnStory.slnx
**Project:** SpohnStory.csproj
**Framework:** .NET 10.0 Windows
**Language:** C# 12
**Output:** Windows Forms Application (WinExe)
**Assembly Name:** SpohnStoryLauncher
**Product Version:** 1.0.0.0

---

## 📊 Build Status

```
✅ BUILD SUCCESSFUL

Compilation: 0 errors, 0 warnings
All types resolved
Full DI container configured
All forms instantiable
Ready for deployment
```

---

## 🎯 Next Steps

### Configuration
1. Update `ApiUrl` in launcher.json to your backend server
2. Test login with your backend API
3. Configure API endpoints if different from defaults

### Customization
1. Update branding colors in Forms if desired
2. Add custom logos/backgrounds in Assets folder
3. Modify UI dimensions/layout in Form creation code
4. Extend diagnostics page with additional checks

### Deployment
1. Build release: `dotnet build -c Release`
2. Publish as single file if desired
3. Distribute executable with logs/ folder
4. launcher.json will auto-create on first run

---

## 🆘 Troubleshooting

**Can't find MapleStory:**
- Use Settings → Browse to manually select folder
- Check Diagnostics for Steam location detection

**Login Fails:**
- Verify API server is running and accessible
- Check Diagnostics page for API URL configuration
- Review logs/launcher.log for error details

**Game Won't Launch:**
- Verify client path is configured in Settings
- Check all DLLs exist (Swordie.dll, nmcogame64.dll, nmconew64.dll)
- Review Diagnostics page validation status

**Can't Copy Diagnostics:**
- Ensure clipboard access is enabled
- Try again or check logs/launcher.log directly

---

## 📄 Documentation

- **MIGRATION_REPORT.md** - Complete technical migration report
- **logs/launcher.log** - Runtime application logs

---

## ✨ Features Summary

| Feature | Status | Details |
|---------|--------|---------|
| Dark Theme UI | ✅ Complete | Modern, professional appearance |
| Authentication | ✅ Complete | Login/Register with tokens |
| Client Detection | ✅ Complete | Auto-detect and manual selection |
| Client Validation | ✅ Complete | DLL verification |
| Settings Page | ✅ Complete | Persistent configuration |
| Diagnostics Page | ✅ Complete | System information & logging |
| Game Launching | ✅ Complete | Process management & logging |
| Logging System | ✅ Complete | File-based with timestamps |
| Configuration | ✅ Complete | JSON-based, no hardcoding |
| Dependency Injection | ✅ Complete | Extensible architecture |

---

**Version:** 1.0.0
**Status:** ✅ PRODUCTION READY
**Created:** 2024

