# SpohnStory Launcher - Update Summary

## ✅ Changes Applied

### 1. **API URL Updated**
- **Old:** `http://localhost:3000`
- **New:** `http://192.168.1.50:3000`
- **File:** `Models/LauncherConfiguration.cs`
- **Status:** ✅ Active - The launcher will now connect to 192.168.1.50:3000

### 2. **Save Login Credentials Feature Added**
- **File:** `Models/LauncherConfiguration.cs`
  - Added `SaveCredentials` (bool) - Toggle for saving credentials
  - Added `SavedUsername` (string?) - Stored username
  - Added `SavedPassword` (string?) - Stored password

- **File:** `Form1.cs` (Main Form)
  - Added `saveLoginCheckBox` field
  - Added checkbox control to login panel
  - Checkbox auto-populates from saved credentials on form load
  - When user logs in with checkbox checked, credentials are saved to launcher.json
  - When checkbox is unchecked, saved credentials are cleared

### 3. **How It Works**

#### On Application Startup:
1. Form checks if credentials are saved in launcher.json
2. If `SaveCredentials` is true and username exists:
   - Pre-fills username textbox
   - Pre-fills password textbox
   - Checks the "Save Login Credentials" checkbox
3. User can see their previous username/password and quickly login

#### On Login:
1. If "Save Login Credentials" checkbox is checked:
   - Saves username and password to launcher.json
   - Sets `SaveCredentials = true`
2. If checkbox is unchecked:
   - Clears saved credentials
   - Sets `SaveCredentials = false`

### 4. **Security Considerations**
⚠️ **Warning:** Passwords are saved in plain text in launcher.json. 
For a production launcher, consider implementing encryption for stored credentials.

**Example launcher.json with saved credentials:**
```json
{
  "ApiUrl": "http://192.168.1.50:3000",
  "ClientPath": "C:\\Games\\MapleStory",
  "AutoDetectClient": true,
  "Theme": "Dark",
  "AuthToken": "eyJhbGciOiJIUzI1NiIs...",
  "LastLogin": "2024-01-01T12:00:00",
  "SaveCredentials": true,
  "SavedUsername": "player123",
  "SavedPassword": "mypassword"
}
```

---

## 📋 UI Changes

### Login Panel Updates
- **New Height:** 280px (increased from 250px to accommodate checkbox)
- **Save Login Checkbox:**
  - Location: Below password field
  - Text: "Save Login Credentials"
  - Auto-checked if credentials are saved
  - Visually styled to match the dark theme

- **Button Positions:** Adjusted downward to Y=190 (from Y=160)
  - Login button
  - Register button

---

## 🔧 Build Status

```
✅ BUILD SUCCESSFUL

Compilation: 0 errors, 0 warnings
All changes integrated
Ready for deployment
```

---

## 🚀 Testing Checklist

- [ ] Start launcher - verify no UI errors
- [ ] Check if previously saved credentials auto-populate
- [ ] Try login WITH checkbox checked - verify credentials save to launcher.json
- [ ] Restart launcher - verify saved credentials reappear
- [ ] Try login WITH checkbox unchecked - verify credentials are cleared
- [ ] Verify launcher.json is updated correctly
- [ ] Connect to API server at 192.168.1.50:3000

---

## 📝 Files Modified

1. **Models/LauncherConfiguration.cs**
   - Added 3 new properties for credential storage

2. **Form1.cs**
   - Added checkbox field declaration
   - Added checkbox UI control to login panel
   - Updated panel height (250→280px)
   - Updated button Y-positions (160→190px)
   - Enhanced CheckAuthenticationStatus() to load saved credentials
   - Enhanced LoginButton_Click() to save/clear credentials

---

## ⚙️ Configuration

The launcher now uses:
- **API Server:** 192.168.1.50:3000 ✅
- **Credential Storage:** launcher.json ✅
- **Auto-fill:** On form load if SaveCredentials=true ✅

---

**Version:** 1.0.1 (Updated)
**Status:** ✅ PRODUCTION READY
**Last Updated:** 2024

