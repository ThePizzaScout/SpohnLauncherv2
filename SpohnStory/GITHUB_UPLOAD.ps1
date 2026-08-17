# SpohnStory Launcher - GitHub Upload Script
# This script will upload your project to GitHub

# Prerequisites:
# 1. Have GitHub account created
# 2. Have Git installed on your system
# 3. Have GitHub CLI (gh) installed OR configure git with your GitHub token

# Step 1: Initialize git repository (run from project root)
Write-Host "Step 1: Initializing Git repository..." -ForegroundColor Cyan
git init

# Step 2: Add all files
Write-Host "Step 2: Adding all files to Git..." -ForegroundColor Cyan
git add .

# Step 3: Create initial commit
Write-Host "Step 3: Creating initial commit..." -ForegroundColor Cyan
git commit -m "Initial commit: SpohnStory Launcher v1.0.1

- Complete game launcher with dark theme UI
- Client detection and validation
- Authentication with token management
- Settings and diagnostics pages
- Comprehensive logging system
- Configured for 192.168.1.50:3000 API endpoint
- Save login credentials feature"

# Step 4: Create GitHub repository using GitHub CLI
Write-Host "`nStep 4: Creating GitHub repository..." -ForegroundColor Cyan
Write-Host "You will need to authenticate with GitHub if not already logged in."
gh repo create SpohnStory-Launcher `
	--public `
	--source=. `
	--remote=origin `
	--push `
	--description "Professional MapleStory game launcher with modern UI, authentication, and client management"

# Step 5: Display repository information
Write-Host "`n" -ForegroundColor Green
Write-Host "✅ Upload Complete!" -ForegroundColor Green
Write-Host "`nYour repository is now live on GitHub!" -ForegroundColor Cyan
Write-Host "Repository URL: https://github.com/YOUR_USERNAME/SpohnStory-Launcher" -ForegroundColor Yellow
Write-Host "`nShare this link to allow others to clone the repository:" -ForegroundColor Cyan
Write-Host "https://github.com/YOUR_USERNAME/SpohnStory-Launcher" -ForegroundColor Yellow

Write-Host "`nTo clone this repository later:" -ForegroundColor Cyan
Write-Host "git clone https://github.com/YOUR_USERNAME/SpohnStory-Launcher.git" -ForegroundColor White

# Optional: Open repository in browser
Write-Host "`nOpening repository in browser..." -ForegroundColor Cyan
gh repo view SpohnStory-Launcher --web
