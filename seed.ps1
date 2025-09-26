# PowerShell Script to apply database seeding fixes to the Contoso University project.
# This script should be run from the root directory of the project.

Write-Host "Starting Contoso University Seeding Fix..." -ForegroundColor Yellow

# --- Define File Paths ---
$projectRoot = $PSScriptRoot
$programCsPath = Join-Path $projectRoot "Program.cs"
$csprojPath = Join-Path $projectRoot "ContosoUniversity.csproj" # Adjust if your csproj has a different name
$seedDataPath = "Data\SeedData.xml" # Relative path used in the csproj

# --- 1. Modify Program.cs to call the DbInitializer ---

Write-Host "Checking Program.cs..."
if (-not (Test-Path $programCsPath)) {
    Write-Host "ERROR: Program.cs not found at '$programCsPath'. Exiting." -ForegroundColor Red
    exit
}

$programCsContent = Get-Content $programCsPath -Raw

# The code block to insert for seeding
$seederCode = @"
// This is the new code to add
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SchoolContext>();
        // context.Database.Migrate(); // Ensures the database is created and migrated
        ContosoUniversity.Data.DbInitializer.InitializeFromXml(context, "Data/SeedData.xml");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the DB.");
    }
}
// End of new code
"@

# Check if the seeding code is already present
if ($programCsContent -like "*InitializeFromXml*") {
    Write-Host "Seeding call already exists in Program.cs. No changes made." -ForegroundColor Green
} else {
    Write-Host "Injecting seeder logic into Program.cs..."
    # Find the target line to insert after
    $targetLine = "var app = builder.Build();"
    $insertionPoint = "$targetLine`r`n`r`n$seederCode"

    # Replace the target line with the target line PLUS our new code
    $newProgramCsContent = $programCsContent -replace [regex]::Escape($targetLine), $insertionPoint

    # Save the updated content back to Program.cs
    Set-Content -Path $programCsPath -Value $newProgramCsContent
    Write-Host "Successfully modified Program.cs." -ForegroundColor Green
}


# --- 2. Modify .csproj to ensure SeedData.xml is copied ---

Write-Host "Checking project file ($csprojPath)..."
if (-not (Test-Path $csprojPath)) {
    Write-Host "ERROR: Project file not found at '$csprojPath'. Exiting." -ForegroundColor Red
    exit
}

$csprojContent = Get-Content $csprojPath -Raw

# The XML block to ensure the file is copied
$copyItemXml = @"
  <ItemGroup>
    <Content Update="Data\SeedData.xml">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
"@

# Check if the configuration already exists
if ($csprojContent -like "*<Content Update=""$seedDataPath""*") {
    Write-Host "SeedData.xml 'Copy to Output' setting already exists in project file. No changes made." -ForegroundColor Green
} else {
    Write-Host "Adding 'Copy to Output' setting for SeedData.xml to project file..."
    # Find the closing </Project> tag and insert our XML block before it
    $newCsprojContent = $csprojContent -replace "</Project>", "`r`n$copyItemXml`r`n</Project>"

    # Save the updated content back to the .csproj file
    Set-Content -Path $csprojPath -Value $newCsprojContent
    Write-Host "Successfully modified project file." -ForegroundColor Green
}

Write-Host "Script finished. Your project is now configured for database seeding." -ForegroundColor Yellow
