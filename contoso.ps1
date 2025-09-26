# Set the project root
$projectRoot = "C:\Users\Joseph\ContosoUniversity"
$dataFolder = Join-Path $projectRoot "Data"

# Ensure Data folder exists
if (!(Test-Path $dataFolder)) { New-Item -Path $dataFolder -ItemType Directory | Out-Null }

# ==========================
# Program.cs
# ==========================
$programFile = Join-Path $projectRoot "Program.cs"
$programContent = @"
using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

// Seed the database from XML
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SchoolContext>();
    context.Database.EnsureCreated();

    var xmlFile = Path.Combine(builder.Environment.ContentRootPath, "Data", "SeedData.xml");
    DbInitializer.InitializeFromXml(context, xmlFile);
}

app.Run();
"@
Set-Content -Path $programFile -Value $programContent -Force

# ==========================
# DbInitializer.cs
# ==========================
$dbInitializerFile = Join-Path $dataFolder "DbInitializer.cs"
$dbInitializerContent = @"
using ContosoUniversity.Models;
using System;
using System.Linq;
using System.Xml.Linq;

namespace ContosoUniversity.Data;

public static class DbInitializer
{
    public static void InitializeFromXml(SchoolContext context, string xmlFile)
    {
        context.Database.EnsureCreated();
        var doc = XDocument.Load(xmlFile);

        // --- Students ---
        var studentsXml = doc.Root?.Element("Students")?.Elements("Student");
        if (studentsXml != null)
        {
            foreach (var s in studentsXml)
            {
                if (!context.Students.Any(st => st.FirstMidName == s.Element("FirstMidName")?.Value &&
                                                st.LastName == s.Element("LastName")?.Value))
                {
                    context.Students.Add(new Student
                    {
                        FirstMidName = s.Element("FirstMidName")?.Value ?? "",
                        LastName = s.Element("LastName")?.Value ?? "",
                        EnrollmentDate = DateTime.Parse(s.Element("EnrollmentDate")?.Value ?? DateTime.MinValue.ToString())
                    });
                }
            }
        }

        // --- Instructors ---
        var instructorsXml = doc.Root?.Element("Instructors")?.Elements("Instructor");
        if (instructorsXml != null)
        {
            foreach (var i in instructorsXml)
            {
                if (!context.Instructors.Any(ins => ins.FirstMidName == i.Element("FirstMidName")?.Value &&
                                                    ins.LastName == i.Element("LastName")?.Value))
                {
                    context.Instructors.Add(new Instructor
                    {
                        FirstMidName = i.Element("FirstMidName")?.Value ?? "",
                        LastName = i.Element("LastName")?.Value ?? "",
                        HireDate = DateTime.Parse(i.Element("HireDate")?.Value ?? DateTime.MinValue.ToString())
                    });
                }
            }
        }

        context.SaveChanges();

        // --- Departments ---
        var departmentsXml = doc.Root?.Element("Departments")?.Elements("Department");
        if (departmentsXml != null)
        {
            foreach (var d in departmentsXml)
            {
                if (!context.Departments.Any(dep => dep.Name == d.Element("Name")?.Value))
                {
                    var adminLastName = d.Element("Administrator")?.Value ?? "";
                    var admin = context.Instructors.FirstOrDefault(ins => ins.LastName == adminLastName);
                    context.Departments.Add(new Department
                    {
                        Name = d.Element("Name")?.Value ?? "",
                        Budget = decimal.Parse(d.Element("Budget")?.Value ?? "0"),
                        StartDate = DateTime.Parse(d.Element("StartDate")?.Value ?? DateTime.MinValue.ToString()),
                        Administrator = admin
                    });
                }
            }
        }

        context.SaveChanges();

        // --- Courses ---
        var coursesXml = doc.Root?.Element("Courses")?.Elements("Course");
        if (coursesXml != null)
        {
            foreach (var c in coursesXml)
            {
                if (!context.Courses.Any(cs => cs.CourseID == int.Parse(c.Element("CourseID")?.Value ?? "0")))
                {
                    var deptName = c.Element("Department")?.Value ?? "";
                    var dept = context.Departments.FirstOrDefault(d => d.Name == deptName);
                    context.Courses.Add(new Course
                    {
                        CourseID = int.Parse(c.Element("CourseID")?.Value ?? "0"),
                        Title = c.Element("Title")?.Value ?? "",
                        Credits = int.Parse(c.Element("Credits")?.Value ?? "0"),
                        Department = dept
                    });
                }
            }
        }

        context.SaveChanges();

        // --- OfficeAssignments ---
        var officesXml = doc.Root?.Element("OfficeAssignments")?.Elements("OfficeAssignment");
        if (officesXml != null)
        {
            foreach (var o in officesXml)
            {
                int instructorId = int.Parse(o.Element("InstructorID")?.Value ?? "0");
                if (!context.OfficeAssignments.Any(oa => oa.InstructorID == instructorId))
                {
                    context.OfficeAssignments.Add(new OfficeAssignment
                    {
                        InstructorID = instructorId,
                        Location = o.Element("Location")?.Value ?? ""
                    });
                }
            }
        }

        // --- CourseAssignments ---
        var courseAssignmentsXml = doc.Root?.Element("CourseAssignments")?.Elements("CourseAssignment");
        if (courseAssignmentsXml != null)
        {
            foreach (var ca in courseAssignmentsXml)
            {
                int courseId = int.Parse(ca.Element("CourseID")?.Value ?? "0");
                int instructorId = int.Parse(ca.Element("InstructorID")?.Value ?? "0");
                if (!context.CourseAssignments.Any(caDb => caDb.CourseID == courseId && caDb.InstructorID == instructorId))
                {
                    context.CourseAssignments.Add(new CourseAssignment
                    {
                        CourseID = courseId,
                        InstructorID = instructorId
                    });
                }
            }
        }

        // --- Enrollments ---
        var enrollmentsXml = doc.Root?.Element("Enrollments")?.Elements("Enrollment");
        if (enrollmentsXml != null)
        {
            foreach (var e in enrollmentsXml)
            {
                var student = context.Students.FirstOrDefault(s => s.FirstMidName == e.Element("StudentFirst")?.Value &&
                                                                   s.LastName == e.Element("StudentLast")?.Value);
                var course = context.Courses.FirstOrDefault(c => c.CourseID == int.Parse(e.Element("CourseID")?.Value ?? "0"));
                if (student != null && course != null &&
                    !context.Enrollments.Any(en => en.StudentID == student.ID && en.CourseID == course.CourseID))
                {
                    context.Enrollments.Add(new Enrollment
                    {
                        StudentID = student.ID,
                        CourseID = course.CourseID,
                        Grade = Enum.TryParse<Grade>(e.Element("Grade")?.Value ?? "", out var gradeVal) ? gradeVal : null
                    });
                }
            }
        }

        context.SaveChanges();
    }
}
"@
Set-Content -Path $dbInitializerFile -Value $dbInitializerContent -Force

# ==========================
# SeedData.xml
# ==========================
$seedFile = Join-Path $dataFolder "SeedData.xml"
$seedContent = @"
<School>
  <Students>
    <Student><FirstMidName>Carson</FirstMidName><LastName>Alexander</LastName><EnrollmentDate>2019-09-01</EnrollmentDate></Student>
    <Student><FirstMidName>Meredith</FirstMidName><LastName>Alonso</LastName><EnrollmentDate>2017-09-01</EnrollmentDate></Student>
    <Student><FirstMidName>Arturo</FirstMidName><LastName>Anand</LastName><EnrollmentDate>2018-09-01</EnrollmentDate></Student>
    <Student><FirstMidName>Gytis</FirstMidName><LastName>Barzdukas</LastName><EnrollmentDate>2017-09-01</EnrollmentDate></Student>
    <Student><FirstMidName>Yan</FirstMidName><LastName>Li</LastName><EnrollmentDate>2017-09-01</EnrollmentDate></Student>
    <Student><FirstMidName>Peggy</FirstMidName><LastName>Justice</LastName><EnrollmentDate>2016-09-01</EnrollmentDate></Student>
    <Student><FirstMidName>Laura</FirstMidName><LastName>Norman</LastName><EnrollmentDate>2018-09-01</EnrollmentDate></Student>
    <Student><FirstMidName>Nino</FirstMidName><LastName>Olivetto</LastName><EnrollmentDate>2019-09-01</EnrollmentDate></Student>
  </Students>
  <Instructors>
    <Instructor><FirstMidName>Kim</FirstMidName><LastName>Abercrombie</LastName><HireDate>1995-03-11</HireDate></Instructor>
    <Instructor><FirstMidName>Fadi</FirstMidName><LastName>Fakhouri</LastName><HireDate>2002-07-06</HireDate></Instructor>
    <Instructor><FirstMidName>Roger</FirstMidName><LastName>Harui</LastName><HireDate>1998-07-01</HireDate></Instructor>
    <Instructor><FirstMidName>Candace</FirstMidName><LastName>Kapoor</LastName><HireDate>2001-01-15</HireDate></Instructor>
    <Instructor><FirstMidName>Roger</FirstMidName><LastName>Zheng</LastName><HireDate>2004-02-12</HireDate></Instructor>
  </Instructors>
  <Departments>
    <Department><Name>English</Name><Budget>350000</Budget><StartDate>2007-09-01</StartDate><Administrator>Abercrombie</Administrator></Department>
    <Department><Name>Mathematics</Name><Budget>100000</Budget><StartDate>2007-09-01</StartDate><Administrator>Fakhouri</Administrator></Department>
    <Department><Name>Engineering</Name><Budget>350000</Budget><StartDate>2007-09-01</StartDate><Administrator>Harui</Administrator></Department>
    <Department><Name>Economics</Name><Budget>100000</Budget><StartDate>2007-09-01</StartDate><Administrator>Kapoor</Administrator></Department>
  </Departments>
  <Courses>
    <Course><CourseID>1050</CourseID><Title>Chemistry</Title><Credits>3</Credits><Department>Engineering</Department></Course>
    <Course><CourseID>4022</CourseID><Title>Microeconomics</Title><Credits>3</Credits><Department>Economics</Department></Course>
    <Course><CourseID>4041</CourseID><Title>Macroeconomics</Title><Credits>3</Credits><Department>Economics</Department></Course>
    <Course><CourseID>1045</CourseID><Title>Calculus</Title><Credits>4</Credits><Department>Mathematics</Department></Course>
    <Course><CourseID>3141</CourseID><Title>Trigonometry</Title><Credits>4</Credits><Department>Mathematics</Department></Course>
    <Course><CourseID>2021</CourseID><Title>Composition</Title><Credits>3</Credits><Department>English</Department></Course>
    <Course><CourseID>2042</CourseID><Title>Literature</Title><Credits>4</Credits><Department>English</Department></Course>
  </Courses>
  <OfficeAssignments>
    <OfficeAssignment><InstructorID>1</InstructorID><Location>Smith 101</Location></OfficeAssignment>
    <OfficeAssignment><InstructorID>2</InstructorID><Location>Gates 202</Location></OfficeAssignment>
  </OfficeAssignments>
  <CourseAssignments>
    <CourseAssignment><CourseID>1050</CourseID><InstructorID>1</InstructorID></CourseAssignment>
    <CourseAssignment><CourseID>1045</CourseID><InstructorID>2</InstructorID></CourseAssignment>
  </CourseAssignments>
  <Enrollments>
    <Enrollment><StudentFirst>Carson</StudentFirst><StudentLast>Alexander</StudentLast><CourseID>1050</CourseID><Grade>A</Grade></Enrollment>
    <Enrollment><StudentFirst>Meredith</StudentFirst><StudentLast>Alonso</StudentLast><CourseID>4022</CourseID><Grade>C</Grade></Enrollment>
    <Enrollment><StudentFirst>Arturo</StudentFirst><StudentLast>Anand</StudentLast><CourseID>4041</CourseID><Grade>B</Grade></Enrollment>
  </Enrollments>
</School>
"@
Set-Content -Path $seedFile -Value $seedContent -Force

Write-Host "All files overwritten with Week 6 data including Enrollments, OfficeAssignments, and CourseAssignments successfully."
