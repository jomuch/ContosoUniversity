#
# Comprehensive Fix-It Script for Week 7
# This script is designed to fix a broken test project setup by:
# 1. Cleaning previous build artifacts.
# 2. Ensuring the test project exists and is correctly referenced.
# 3. Installing all required NuGet packages.
# 4. Deleting all existing C# files in the test project to remove duplicates.
# 5. Re-creating the correct test files from scratch.
# 6. Applying all other necessary code changes for the assignment.
#

# --- Script Configuration ---
$SolutionFile = "ContosoUniversity.sln"
$MainProjectFolder = "ContosoUniversity"
$MainProjectFile = "$MainProjectFolder\ContosoUniversity.csproj"
$TestProjectFolder = "ContosoUniversity.Tests"
$TestProjectFile = "$TestProjectFolder\ContosoUniversity.Tests.csproj"

# --- Helper Functions ---
function Write-Header($message) {
    Write-Host " "
    Write-Host "================================================================" -ForegroundColor Cyan
    Write-Host "  $message" -ForegroundColor Cyan
    Write-Host "================================================================" -ForegroundColor Cyan
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

Write-Header "Starting Comprehensive Project Fix for Week 7"

# Verify we are in the correct directory
if (-not (Test-Path $SolutionFile)) {
    Write-Error-Exit "Solution file '$SolutionFile' not found. Please run this script from the root directory of your solution."
}

# 1. Clean the solution
Write-Info "Cleaning the solution to remove old build artifacts..."
dotnet clean | Out-Null
Write-Success "Solution cleaned."

# 2. Create Test Project if it doesn't exist
if (-not (Test-Path $TestProjectFile)) {
    Write-Info "Test project not found. Creating '$TestProjectFolder'..."
    dotnet new xunit -n $TestProjectFolder
    dotnet sln add $TestProjectFile
    Write-Success "Successfully created and added test project to solution."
}
else {
    Write-Info "Test project '$TestProjectFile' already exists."
}

# 3. Add Project Reference
Write-Info "Verifying project reference..."
$referenceExists = dotnet list "$TestProjectFile" reference | Select-String $MainProjectFolder
if (-not $referenceExists) {
    dotnet add $TestProjectFile reference $MainProjectFile
    Write-Success "Added reference from test project to main project."
}
else {
    Write-Info "Project reference already exists."
}

# 4. Install NuGet Packages
Write-Header "Installing Required NuGet Packages"
$packages = @(
    "Microsoft.NET.Test.Sdk",
    "xunit",
    "xunit.runner.visualstudio",
    "Microsoft.AspNetCore.Mvc.Testing",
    "Microsoft.EntityFrameworkCore.InMemory",
    "Moq"
)

foreach ($pkg in $packages) {
    Write-Info "Installing package '$pkg'..."
    dotnet add $TestProjectFile package $pkg | Out-Null
}
Write-Success "All required NuGet packages are installed in the test project."

# 5. Refactor DbExporter and update Program.cs
Write-Header "Applying Changes to Main Application"
# Create IDbExportService.cs
$interfacePath = Join-Path $MainProjectFolder "Data\IDbExportService.cs"
$interfaceContent = @"
namespace ContosoUniversity.Data { public interface IDbExportService { System.Xml.Linq.XDocument ExportToXml(); } }
"@
Set-Content -Path $interfacePath -Value $interfaceContent
Write-Success "Created IDbExportService.cs"

# Create DbExportService.cs
$servicePath = Join-Path $MainProjectFolder "Data\DbExportService.cs"
$serviceContent = @"
using ContosoUniversity.Models; using System.Linq; using System.Xml.Linq;
namespace ContosoUniversity.Data {
    public class DbExportService : IDbExportService {
        private readonly SchoolContext _context;
        public DbExportService(SchoolContext context) { _context = context; }
        public XDocument ExportToXml() {
            var students = _context.Students.ToList();
            return new XDocument( new XElement("School", new XElement("Students", from s in students
                select new XElement("student", new XElement("ID", s.ID), new XElement("LastName", s.LastName),
                new XElement("FirstMidName", s.FirstMidName), new XElement("EnrollmentDate", s.EnrollmentDate.ToString("yyyy-MM-dd"))))));
        }
    }
}
"@
Set-Content -Path $servicePath -Value $serviceContent
Write-Success "Created DbExportService.cs"

# Update Program.cs
$programCsPath = Join-Path $MainProjectFolder "Program.cs"
$programCsContent = Get-Content $programCsPath -Raw
$serviceRegistration = "builder.Services.AddScoped<ContosoUniversity.Data.IDbExportService, ContosoUniversity.Data.DbExportService>();"
$anchorLine = "builder.Services.AddDatabaseDeveloperPageExceptionFilter();"
if ($programCsContent -notmatch [regex]::Escape($serviceRegistration)) {
    $programCsContent = $programCsContent -replace [regex]::Escape($anchorLine), "$anchorLine`n$serviceRegistration"
    Write-Success "Registered IDbExportService in Program.cs"
}
$partialClass = "public partial class Program { }"
if ($programCsContent -notmatch [regex]::Escape($partialClass)) {
    $programCsContent = $programCsContent + "`n`n// Make the Program class visible to the test project`n" + $partialClass
    Write-Success "Made Program class visible to the test project."
}
Set-Content -Path $programCsPath -Value $programCsContent

# 6. Clean and Recreate Test Files
Write-Header "Cleaning and Re-creating Test Files"
# *** FIX: Delete all existing .cs files in the test project to remove duplicates ***
$csFiles = Get-ChildItem -Path $TestProjectFolder -Filter "*.cs"
if ($csFiles) {
    Write-Info "Deleting existing C# files from test project to prevent duplicates..."
    Remove-Item $csFiles.FullName -Force
    Write-Success "Old test files removed."
}

# Create DbExportServiceTests.cs
$unitTestPath = Join-Path $TestProjectFolder "DbExportServiceTests.cs"
$unitTestContent = @"
using ContosoUniversity.Data; using ContosoUniversity.Models; using Microsoft.EntityFrameworkCore; using Moq; using System; using System.Linq; using System.Xml.Linq; using Xunit;
namespace ContosoUniversity.Tests {
    public class DbExportServiceTests {
        private readonly DbContextOptions<SchoolContext> _options;
        public DbExportServiceTests() {
            _options = new DbContextOptionsBuilder<SchoolContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        }
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
            response.EnsureSuccessStatusCode(); 
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
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

# 7. Add Structured Logging
Write-Header "Adding Structured Logging to Student Details Page"
$studentsPagesFolder = Join-Path $MainProjectFolder "Pages\Students"
if (-not (Test-Path $studentsPagesFolder)) { New-Item -ItemType Directory -Path $studentsPagesFolder | Out-Null }
$detailsPageModelPath = Join-Path $studentsPagesFolder "Details.cshtml.cs"
$detailsPageModelContent = @"
using ContosoUniversity.Data; using ContosoUniversity.Models; using Microsoft.AspNetCore.Mvc; using Microsoft.AspNetCore.Mvc.RazorPages; using Microsoft.EntityFrameworkCore; using Microsoft.Extensions.Logging; using System.Threading.Tasks;
namespace ContosoUniversity.Pages.Students {
    public class DetailsModel : PageModel {
        private readonly SchoolContext _context; private readonly ILogger<DetailsModel> _logger;
        public DetailsModel(SchoolContext context, ILogger<DetailsModel> logger) { _context = context; _logger = logger; }
        public Student Student { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) { return NotFound(); }
            _logger.LogInformation("Getting details for Student ID: {StudentId} at {Time}", id, System.DateTime.UtcNow);
            Student = await _context.Students.Include(s => s.Enrollments).ThenInclude(e => e.Course).AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            if (Student == null) { _logger.LogWarning("Student with ID: {StudentId} not found.", id); return NotFound(); }
            return Page();
        }
    }
}
"@
Set-Content -Path $detailsPageModelPath -Value $detailsPageModelContent
Write-Success "Updated Student Details page with structured logging."

Write-Header "Week 7 Fix is Complete!"
Write-Host "Please REBUILD your solution in Visual Studio now." -ForegroundColor Green
Write-Host "Then, open the 'Test Explorer' and run the tests." -ForegroundColor Green

