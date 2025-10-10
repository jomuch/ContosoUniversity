# ==========================================
# Setup-Week8Configs.ps1
# Creates environment-specific config files
# for ContosoUniversity (Week 8)
# ==========================================

# Ensure we are in the project root
Write-Host "🔍 Checking current directory..." -ForegroundColor Cyan
if (-not (Test-Path "appsettings.json")) {
    Write-Host "❌ Please run this script from the project root (where appsettings.json is located)." -ForegroundColor Red
    exit 1
}

# --- File paths ---
$devFile = "appsettings.Development.json"
$prodFile = "appsettings.Production.json"

# --- Development config ---
$devConfig = @'
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "SchoolContext": "Server=(localdb)\\\\mssqllocaldb;Database=ContosoUniversity_Dev;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
'@

# --- Production config ---
$prodConfig = @'
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Error",
      "Microsoft.Hosting.Lifetime": "Warning"
    }
  },
  "ConnectionStrings": {
    "SchoolContext": "Server=tcp:prod-sqlserver.database.windows.net,1433;Initial Catalog=ContosoUniversity_Prod;Persist Security Info=False;User ID=YOUR_SQL_USER;Password=USE_SECRETS;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
'@

# --- Write Development file ---
if (Test-Path $devFile) {
    Write-Host "⚠️  $devFile already exists. Skipping creation." -ForegroundColor Yellow
} else {
    $devConfig | Out-File -FilePath $devFile -Encoding UTF8
    Write-Host "✅ Created $devFile" -ForegroundColor Green
}

# --- Write Production file ---
if (Test-Path $prodFile) {
    Write-Host "⚠️  $prodFile already exists. Skipping creation." -ForegroundColor Yellow
} else {
    $prodConfig | Out-File -FilePath $prodFile -Encoding UTF8
    Write-Host "✅ Created $prodFile" -ForegroundColor Green
}

Write-Host "`n✨ Environment-specific config setup complete!" -ForegroundColor Cyan
Write-Host "➡️ Next steps:"
Write-Host "   1. Run: dotnet user-secrets init" -ForegroundColor DarkGray
Write-Host "   2. Run: dotnet user-secrets set \"ConnectionStrings:SchoolContext\" \"Server=(localdb)\\mssqllocaldb;Database=ContosoUniversity_Secret;Trusted_Connection=True;MultipleActiveResultSets=true\""
Write-Host "   3. Add health endpoint in Program.cs before app.Run():"
Write-Host "      app.MapGet(\"/healthz\", () => Results.Ok(\"OK\"));" -ForegroundColor DarkGray
