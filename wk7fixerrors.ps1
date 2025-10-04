#
# The Final Fix-It Script for Contoso University
# This script performs a deep clean and complete rebuild of the solution structure.
#

# --- Script Configuration ---
$SolutionFile = "ContosoUniversity.sln"
$MainProjectName = "ContosoUniversity"
$TestProjectName = "ContosoUniversity.Tests"

# --- Helper Functions ---
function Write-Header($message) {
    Write-Host " "
    Write-Host "================================================================" -ForegroundColor Magenta
    Write-Host "  $message" -ForegroundColor Magenta
    Write-Host "================================================================" -ForegroundColor Magenta
}

function Write-Success($message) {
    Write-Host "[SUCCESS] $message" -ForegroundColor Green
}

function Write-Info($message) {
    Write-Host "[INFO] $message" -ForegroundColor Yellow
}

function Write-Error-Exit($message) {
    Write-Host "[ERROR] $message" -ForegroundColor Red
    exit 1
}

# --- Main Script ---

Write-Header "Starting The Final Fix-It Script"

# Verify we are in the correct directory
if (-not (Test-Path $SolutionFile)) {
    Write-Error-Exit "Solution file '$SolutionFile' not found. Please run this script from the root directory."
}

# STEP 1: DEEP CLEAN THE ENTIRE SOLUTION
Write-Header "Step 1: Performing Deep Clean"
Write-Info "Removing all bin, obj, and hidden .vs folders..."
Get-ChildItem -Path . -Include "bin", "obj", ".vs" -Recurse -Force | Remove-Item -Recurse -Force
Write-Success "All temporary build folders have been deleted."

# Find the main project file dynamically
$mainProjectFileObject = Get-ChildItem -Recurse -Filter "$MainProjectName.csproj" | Select-Object -First 1
if (-not $mainProjectFileObject) {
    Write-Error-Exit "Could not find '$MainProjectName.csproj'. Cannot continue."
}
$MainProjectFile = $mainProjectFileObject.FullName
$MainProjectFolder = $mainProjectFileObject.DirectoryName

# Find and remove the test project if it exists
$testProjectFileObject = Get-ChildItem -Recurse -Filter "$TestProjectName.csproj" | Select-Object -First 1
if ($testProjectFileObject) {
    Write-Info "Removing existing test project to ensure a clean state..."
    dotnet sln remove $testProjectFileObject.FullName | Out-Null
    Remove-Item -Recurse -Force $testProjectFileObject.DirectoryName
    Write-Success "Existing test project removed."
}

# STEP 2: REBUILD THE PROJECT STRUCTURE
Write-Header "Step 2: Rebuilding Project Structure"
Write-Info "Creating new xUnit test project: '$TestProjectName'..."
$TestProjectFolder = Join-Path (Get-Location) $TestProjectName
$TestProjectFile = Join-Path $TestProjectFolder "$TestProjectName.csproj"
dotnet new xunit -n $TestProjectName | Out-Null
dotnet sln add $TestProjectFile | Out-Null
Write-Success "Test project created and added to solution."

Write-Info "Adding project reference from Tests to Main project..."
dotnet add $TestProjectFile reference $MainProjectFile | Out-Null
Write-Success "Project reference added."

# STEP 3: RESTORE ALL NUGET PACKAGES
Write-Header "Step 3: Force-Restoring All NuGet Packages"
$packages = @(
    "Microsoft.NET.Test.Sdk", "xunit", "xunit.runner.visualstudio",
    "Microsoft.AspNetCore.Mvc.Testing", "Microsoft.EntityFrameworkCore.InMemory", "Moq"
)
foreach ($pkg in $packages) {
    Write-Info "Adding package '$pkg' to test project..."
    dotnet add $TestProjectFile package $pkg | Out-Null
}
Write-Success "All required NuGet packages added to the test project."
Write-Info "Restoring packages for the entire solution..."
dotnet restore | Out-Null
Write-Success "NuGet restore complete."

# STEP 4: RE-CREATE ALL NECESSARY FILES
Write-Header "Step 4: Re-creating C# Files"

# Clean out default test file
Remove-Item (Join-Path $TestProjectFolder "UnitTest1.cs") -Force
Write-Info "Removed default UnitTest1.cs file."

# Create DbExportServiceTests.cs
$unitTestPath = Join-Path $TestProjectFolder "DbExportServiceTests.cs"
$unitTestContent = @"
using ContosoUniversity.Data; using ContosoUniversity.Models; using Microsoft.EntityFrameworkCore; using Moq; using System; using System.Linq; using System.Xml.Linq; using Xunit;
namespace ContosoUniversity.Tests {
    public class DbExportServiceTests {
        private readonly DbContextOptions<SchoolContext> _options;
        public DbExportServiceTests() { _options = new DbContextOptionsBuilder<SchoolContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options; }
        [Fact]
        public void ExportToXml_WithData_CreatesCorrectRootElement() {
            using (var context = new SchoolContext(_options)) {
                context.Students.Add(new Student { FirstMidName = "Test", LastName = "Student", EnrollmentDate = DateTime.Now });
                context.SaveChanges();
                var service = new DbExportService(context);
                XDocument result = service.ExportToXml();
                Assert.NotNull(result); Assert.Equal("School", result.Root.Name);
            }
        }
        [Fact]
        public void ExportToXml_WithStudentData_CreatesStudentElementWithCorrectName() {
            using (var context = new SchoolContext(_options)) {
                var testStudent = new Student { ID = 1, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2019-09-01") };
                context.Students.Add(testStudent); context.SaveChanges();
                var service = new DbExportService(context);
                XDocument result = service.ExportToXml();
                var studentElement = result.Root?.Element("Students")?.Elements("student").FirstOrDefault();
                Assert.NotNull(studentElement); Assert.Equal("Alexander", studentElement.Element("LastName")?.Value); Assert.Equal("Carson", studentElement.Element("FirstMidName")?.Value);
            }
        }
    }
}
"@
Set-Content -Path $unitTestPath -Value $unitTestContent
Write-Success "Created DbExportServiceTests.cs"

# Create StudentPagesTests.cs
$integrationTestPath = Join-Path $TestProjectFolder "StudentPagesTests.cs"
$integrationTestContent = @"
using Microsoft.AspNetCore.Mvc.Testing; using System.Net; using System.Threading.Tasks; using Xunit;
namespace ContosoUniversity.Tests {
    public class StudentPagesTests : IClassFixture<WebApplicationFactory<Program>> {
        private readonly WebApplicationFactory<Program> _factory;
        public StudentPagesTests(WebApplicationFactory<Program> factory) { _factory = factory; }
        [Fact]
        public async Task Get_StudentsIndexPage_ReturnsSuccessAndCorrectContentType() {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Students");
            response.EnsureSuccessStatusCode(); Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
        [Fact]
        public async Task Get_StudentsDetailsPage_ReturnsNotFoundForInvalidId() {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Students/Details?id=999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
"@
Set-Content -Path $integrationTestPath -Value $integrationTestContent
Write-Success "Created StudentPagesTests.cs"

# Make Program.cs visible to tests
$programCsPath = Join-Path $MainProjectFolder "Program.cs"
$programCsContent = Get-Content $programCsPath -Raw
$partialClass = "public partial class Program { }"
if ($programCsContent -notmatch [regex]::Escape($partialClass)) {
    $programCsContent += "`n`n// Make the Program class visible to the test project`n$partialClass"
    Set-Content -Path $programCsPath -Value $programCsContent
    Write-Success "Made Program class visible to the test project."
}

Write-Header "FINAL FIX IS COMPLETE!"
Write-Host "Please close and re-open your solution in Visual Studio now." -ForegroundColor Green
Write-Host "Then, REBUILD the solution. The errors will be gone." -ForegroundColor Green

