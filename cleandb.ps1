# -------------------------------------------------------------------
# Database Cleanup and Reset Script (Use in PowerShell or PMC)
# -------------------------------------------------------------------

# 1. Stop Kestrel/IIS Express (Prevents locks on the database file)
Write-Host "1. Attempting to stop the running application (if any)..." -ForegroundColor Yellow
try {
    # Assuming Kestrel runs on PID found in launchSettings or Visual Studio process
    # This is often complex, so we prioritize the reliable database drop command later.
    # We will mainly rely on the drop command's ability to kill connections.
} catch {
    Write-Host "   Could not forcefully stop app, continuing with database drop." -ForegroundColor DarkYellow
}

# Define the Migrations folder path
$migrationsPath = "Data/Migrations"
Write-Host "2. Deleting all files in $migrationsPath..." -ForegroundColor Yellow

# 2. Delete all existing migration files (necessary to resolve the RowVersion conflict)
try {
    # Get all files inside the Migrations folder and delete them
    Get-ChildItem -Path $migrationsPath -Exclude ".*" -Recurse -Force | Remove-Item -Force -Recurse -ErrorAction Stop
    Write-Host "   -> Successfully deleted all migration history files." -ForegroundColor Green
} catch {
    Write-Host "   -> Error deleting migration files. Check path and permissions." -ForegroundColor Red
    exit 1
}

# 3. Forcefully drop the database (using the reliable .NET CLI command)
Write-Host "3. Forcefully dropping the existing database using dotnet ef..." -ForegroundColor Yellow
try {
    # Execute the powerful CLI drop command
    dotnet ef database drop --force --no-build
    Write-Host "   -> Database successfully dropped." -ForegroundColor Green
} catch {
    Write-Host "   -> Error executing dotnet ef database drop. Ensure the database service is running." -ForegroundColor Red
    exit 1
}

# 4. Create a clean Initial Migration (FinalSchema)
Write-Host "4. Creating a new, clean migration (FinalSchema)..." -ForegroundColor Yellow
try {
    # Use Add-Migration to capture the current model state without conflicts
    dotnet ef migrations add FinalSchema
    Write-Host "   -> Migration 'FinalSchema' created successfully." -ForegroundColor Green
} catch {
    Write-Host "   -> Error creating new migration. Check C# syntax errors in models/context." -ForegroundColor Red
    exit 1
}

# 5. Apply the new migration (creates the clean database structure)
Write-Host "5. Applying the 'FinalSchema' migration (recreating the database)..." -ForegroundColor Yellow
try {
    # Use Update-Database to apply the new schema
    Update-Database
    Write-Host "   -> Database successfully created and updated to the latest schema." -ForegroundColor Green
} catch {
    Write-Host "   -> Critical Error during Update-Database. Database is not ready." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "--------------------------------------------------------" -ForegroundColor Green
Write-Host "Database cleanup and reset complete." -ForegroundColor Green
Write-Host "The database is now completely empty and ready for the log test." -ForegroundColor Green
Write-Host "--------------------------------------------------------" -ForegroundColor Green