# -------------------------------------------------------------------
# WARNING: This script uses string replacement and assumes a specific 
# code structure from the Contoso University tutorial. 
# BACKUP YOUR FILES before running.
# -------------------------------------------------------------------

# Define file paths relative to the project root
$ProgramFile = "Program.cs"
$InitializerFile = "Data/DbInitializer.cs"
$ExporterFile = "Data/DbExporter.cs"

# -------------------------------------------------------------------
# 1. Update DbInitializer.cs Method Signature
# -------------------------------------------------------------------
Write-Host "Updating $InitializerFile..."
(Get-Content $InitializerFile) -replace "public static void InitializeFromXml(SchoolContext context, string xmlFile)", `
                               "public static void InitializeFromXml(SchoolContext context, string xmlFile, ILogger logger)" | Set-Content $InitializerFile
Write-Host "-> Initializer signature updated."

# -------------------------------------------------------------------
# 2. Update DbExporter.cs Method Signature
# -------------------------------------------------------------------
Write-Host "Updating $ExporterFile..."
(Get-Content $ExporterFile) -replace "public static void ExportToXml(SchoolContext context, string exportPath)", `
                               "public static void ExportToXml(SchoolContext context, string exportPath, ILogger logger)" | Set-Content $ExporterFile
Write-Host "-> Exporter signature updated."

# -------------------------------------------------------------------
# 3. Update Program.cs for Logger Injection
# -------------------------------------------------------------------
Write-Host "Updating $ProgramFile..."
$content = Get-Content $ProgramFile -Raw

# --- A. Add using statement if missing ---
if ($content -notmatch "using Microsoft.Extensions.DependencyInjection;") {
    $content = $content -replace "using Microsoft.EntityFrameworkCore;", "using Microsoft.EntityFrameworkCore;`r`nusing Microsoft.Extensions.DependencyInjection;"
    Write-Host "-> Added using Microsoft.Extensions.DependencyInjection;"
}

# --- B. Replace the data initialization block with logger injection ---
# Note: This is a complex multi-line replacement
$scopeBlockPattern = 'using \(var scope = app\.Services\.CreateScope\(\)\) {[\s\S]*?}'
$newScopeBlock = @'
using (var scope = app.Services.CreateScope()) 
{ 
    var services = scope.ServiceProvider; 
    var context = services.GetRequiredService<SchoolContext>(); 
    
    // Retrieve the logger for the DbInitializer class (used for both Init and Export)
    var initializerLogger = services.GetRequiredService<ILogger<DbInitializer>>();
    
    try 
    { 
        // 1. Run Migrations
        context.Database.Migrate(); 
        initializerLogger.LogInformation("Database migrations applied successfully.");
        
        // 2. Seed database from XML 
        string seedFile = Path.Combine(builder.Environment.ContentRootPath, "Data", "SeedData.xml"); 
        if (File.Exists(seedFile)) 
        { 
            // Pass the logger to the initialization method
            DbInitializer.InitializeFromXml(context, seedFile, initializerLogger); 
        } 
        else
        {
            initializerLogger.LogError("Seed file not found at: {SeedPath}", seedFile);
        }
        
        // 3. Export current database to XML 
        string exportFile = Path.Combine(builder.Environment.ContentRootPath, "Data", "ExportedData.xml"); 
        // Pass the logger to the export method
        DbExporter.ExportToXml(context, exportFile, initializerLogger);
    } 
    catch (Exception ex) 
    { 
        initializerLogger.LogError(ex, "An unexpected error occurred during database operations (migration/seeding/export)."); 
    } 
} 
'@

# Find and replace the entire scope block
$content = $content -replace $scopeBlockPattern, $newScopeBlock -replace '\r\n', "`n" -replace '`n', "`r`n"
Set-Content $ProgramFile -Value $content
Write-Host "-> Program.cs initialization block successfully updated."

Write-Host ""
Write-Host "Script execution complete. You should now be able to run your application and see the logs."
