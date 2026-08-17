# How to Upload SpohnStory Launcher to GitHub

## Prerequisites

### 1. GitHub Account
- Create an account at https://github.com if you don't have one
- Free accounts can create unlimited public repositories

### 2. Git Installation
- Download and install Git: https://git-scm.com/download/win
- Verify installation:
  ```powershell
  git --version
  ```

### 3. GitHub CLI (Optional but Recommended)
- Download: https://cli.github.com/
- Or install via Chocolatey:
  ```powershell
  choco install gh
  ```
- Verify installation:
  ```powershell
  gh --version
  ```

---

## Method 1: Using GitHub CLI (Easiest)

### Step 1: Authenticate with GitHub
```powershell
gh auth login
```
Follow the prompts to authenticate.

### Step 2: Navigate to Project Directory
```powershell
cd "C:\Users\Dustin\source\repos\attempt 2\SpohnStory\"
```

### Step 3: Run the Upload Script
```powershell
.\GITHUB_UPLOAD.ps1
```

The script will:
- Initialize git repository
- Add all files
- Create initial commit
- Create GitHub repository
- Push files to GitHub
- Open repository in browser

### Step 4: Share Your Repository Link
After successful upload, your repository will be at:
```
https://github.com/YOUR_USERNAME/SpohnStory-Launcher
```

---

## Method 2: Manual Steps (GitHub Web Interface)

### Step 1: Create Repository on GitHub

1. Go to https://github.com/new
2. Fill in repository details:
   - **Repository name:** `SpohnStory-Launcher`
   - **Description:** "Professional MapleStory game launcher with modern UI"
   - **Public:** ✓ (or Private if you prefer)
   - **Initialize this repository with:** Leave unchecked
3. Click "Create repository"

### Step 2: Push Your Local Files

In PowerShell, navigate to your project:
```powershell
cd "C:\Users\Dustin\source\repos\attempt 2\SpohnStory\"
```

Initialize git:
```powershell
git init
git add .
git commit -m "Initial commit: SpohnStory Launcher v1.0.1"
```

Add remote repository (replace YOUR_USERNAME):
```powershell
git branch -M main
git remote add origin https://github.com/YOUR_USERNAME/SpohnStory-Launcher.git
git push -u origin main
```

You'll be prompted for GitHub credentials or personal access token.

---

## Generating a Personal Access Token

If you get authentication errors:

### Step 1: Create Token
1. Go to https://github.com/settings/tokens
2. Click "Generate new token"
3. Select these scopes:
   - ✓ repo (full control of private repositories)
   - ✓ workflow
4. Click "Generate token"
5. Copy the token (you won't see it again!)

### Step 2: Use Token for Authentication
When Git asks for password, paste the token instead.

---

## Verify Upload

After pushing, verify your repository:

### Via Web Browser
```
https://github.com/YOUR_USERNAME/SpohnStory-Launcher
```

### Via GitHub CLI
```powershell
gh repo view SpohnStory-Launcher --web
```

---

## Share Your Repository

### Public Link to Share
```
https://github.com/YOUR_USERNAME/SpohnStory-Launcher
```

### Clone Command (for others)
```bash
git clone https://github.com/YOUR_USERNAME/SpohnStory-Launcher.git
```

### Download as ZIP
Click "Code" → "Download ZIP" on your GitHub repository page.

---

## Files That Will Be Uploaded

```
SpohnStory/
├── Models/
│   ├── LauncherConfiguration.cs
│   ├── ClientValidationResult.cs
│   ├── LaunchLogEntry.cs
│   ├── AuthenticationResponse.cs
│   ├── DiagnosticState.cs
│   └── .gitkeep
│
├── Services/
│   ├── LoggingService.cs
│   ├── ClientLocatorService.cs
│   ├── ApiClient.cs
│   ├── AuthenticationService.cs
│   ├── LaunchService.cs
│   └── .gitkeep
│
├── Forms/
│   ├── SettingsForm.cs
│   ├── DiagnosticsForm.cs
│   └── .gitkeep
│
├── Configuration/
│   ├── ConfigurationService.cs
│   └── .gitkeep
│
├── Assets/
│   └── .gitkeep
│
├── logs/
│   └── .gitkeep
│
├── Program.cs
├── Form1.cs
├── Form1.Designer.cs
├── Form1.resx
├── SpohnStory.csproj
│
├── README.md
├── QUICK_START.md
├── MIGRATION_REPORT.md
├── FILE_MANIFEST.md
├── UPDATE_SUMMARY.md
├── .gitignore
├── GITHUB_UPLOAD.ps1
└── (This file)
```

**Total:** ~25+ files, all project source code and documentation

---

## Troubleshooting

### Error: "fatal: not a git repository"
**Solution:** Run `git init` in project directory first

### Error: "Authentication failed"
**Solution:** Use personal access token instead of password

### Error: "remote already exists"
**Solution:** Remove existing remote:
```powershell
git remote remove origin
git remote add origin https://github.com/YOUR_USERNAME/SpohnStory-Launcher.git
```

### Large files warning
**Solution:** The project is small (~5MB). If you get warnings, check the .gitignore.

---

## After Upload

### Update Documentation
Update the README.md with your actual GitHub username in the clone command.

### Add Topics (Optional)
1. Go to your repository
2. Click "Settings"
3. Add topics: `launcher`, `maplestory`, `game-launcher`, `dark-theme`, `windows-forms`, `csharp`, `dotnet`

### Enable Discussions (Optional)
1. Go to Settings → Features
2. Enable "Discussions"
3. This allows users to ask questions

### Add a License (Optional)
1. Create a LICENSE file in your repository
2. Choose MIT, GPL, Apache 2.0, etc.

---

## Quick Reference

| Task | Command |
|------|---------|
| Initialize git | `git init` |
| Add all files | `git add .` |
| Create commit | `git commit -m "message"` |
| Add remote | `git remote add origin <url>` |
| Push to GitHub | `git push -u origin main` |
| Check status | `git status` |
| View remote | `git remote -v` |
| Authenticate | `gh auth login` |
| Create repo | `gh repo create <name>` |
| View in browser | `gh repo view <name> --web` |

---

## Next Steps

1. ✅ Create GitHub account (if needed)
2. ✅ Install Git and GitHub CLI
3. ✅ Follow one of the upload methods above
4. ✅ Share your repository link
5. ✅ Update README with any custom information
6. ✅ Invite contributors or share with team

---

## Support

For GitHub help, visit:
- https://docs.github.com/
- https://docs.github.com/en/get-started

For Git help, visit:
- https://git-scm.com/doc
- https://www.atlassian.com/git/tutorials

---

**Your repositories will be at:** `https://github.com/YOUR_USERNAME/SpohnStory-Launcher`

Good luck! 🚀
