$filePath = ".\Services\ICourseService.cs"

# Ensure the Services directory exists
$dirPath = Split-Path -Parent $filePath
if (-not (Test-Path -Path $dirPath -PathType Container)) {
    New-Item -Path $dirPath -ItemType Directory | Out-Null
}

# Content of ICourseService.cs
$fileContent = @"
using ContosoUniversity.Models;
using System.Collections.Generic;

namespace ContosoUniversity.Services
{
    public interface ICourseService
    {
        IEnumerable<Course> GetAllCourses();
        Course? GetCourseById(int id);
    }
}
"@

# Write the content to the file
$fileContent | Out-File -FilePath $filePath -Encoding UTF8

Write-Host "Successfully created interface file at: $filePath"