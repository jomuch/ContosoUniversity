# 1. Clear the global NuGet cache using the correct syntax
dotnet nuget locals all --clear
# Expected output: Cleaning local cache of type 'all': C:\Users\...\AppData\Local\NuGet...

# 2. Restore dependencies again (forces fresh download/metadata regeneration)
dotnet restore