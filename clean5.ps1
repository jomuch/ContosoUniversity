Write-Host "--- Performing deep clean to fix CS0579 error ---"

# Forcefully remove bin and obj directories, suppressing errors if they don't exist.
Remove-Item .\bin -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item .\obj -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item .\ContosoUniversity.Tests\bin -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item .\ContosoUniversity.Tests\obj -Recurse -Force -ErrorAction SilentlyContinue

# Clear the local NuGet cache and restore dependencies
dotnet nuget locals all --clear
dotnet restore

Write-Host "--- Clean complete. Building project and running tests..."

# Build the application and run all tests
dotnet build
Write-Host "--- Build complete. Running tests... ---"
dotnet test

# Run the application
Write-Host "--- Starting application ---"
dotnet run
