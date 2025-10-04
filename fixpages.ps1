$filePath = ".\ContosoUniversity.Tests\DbExportServiceTests.cs"

# Check if the file exists before proceeding
if (-not (Test-Path -Path $filePath)) {
    Write-Host "Error: Test file $filePath not found. Cannot proceed with fix."
    exit 1
}

$content = Get-Content $filePath -Raw

# Add the missing using directive if it's not already present
$usingDirective = "using ContosoUniversity.Services;"
if ($content -notlike "*$usingDirective*") {
    $newContent = "$usingDirective`n$content"
    $newContent | Set-Content $filePath -Encoding UTF8
    Write-Host "Added missing 'using ContosoUniversity.Services;' to $filePath"
} else {
    Write-Host "Using directive already present in $filePath. Skipping."
}