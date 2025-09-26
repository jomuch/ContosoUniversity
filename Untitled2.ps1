# Set the project root
$projectRoot = "C:\Users\Joseph\ContosoUniversity"
$dataFolder = Join-Path $projectRoot "Data"

# Remove old seed files if they exist
$oldFiles = @("schoolseed.xml", "seeddata.xml")
foreach ($file in $oldFiles) {
    $fullPath = Join-Path $dataFolder $file
    if (Test-Path $fullPath) {
        Remove-Item $fullPath -Force
        Write-Host "Removed old seed file: $file"
    }
}

# Confirm remaining files
Write-Host "Remaining XML files in Data folder:"
Get-ChildItem -Path $dataFolder -Filter "*.xml" | Select-Object Name
