Contoso University - Week 6 Submission
Repository & Migration Files
Repository Link: https://github.com/jomuch/ContosoUniversity.git
Migration Files: The migration files can be found in the /Migrations directory of the project repository.
README.md
Week 5 Revision – Student Forms
Overview
This revision implements the Create and Edit functionality for the Student entity in the Contoso University application. Features include:
Model binding via Razor Pages [BindProperty] and ViewModels.
Server-side and client-side validation using Data Annotations and _ValidationScriptsPartial.
PRG (Post-Redirect-Get) pattern to prevent duplicate submissions.
TempData success messages to notify the user after successful creation or edits.
Changes Made
Create.cshtml / Create.cshtml.cs: Added form, ViewModel binding, validation, PRG, and TempData.
Edit.cshtml / Edit.cshtml.cs: Updated to match Create page pattern with PRG and TempData.
Index.cshtml: Added display for TempData success messages.
Test Plan
1. Create Student


Test Case
Steps
Expected Result
Create with valid data
1. Go to "Create Student" page. 2. Enter valid First Name, Last Name, and Enrollment Date. 3. Click "Create".
Student is saved in database. Redirected to Index page. Success message displays.
Create with missing required field
1. Leave Last Name or First Name empty. 2. Click "Create".
Validation error displays next to empty field. Student is not saved.
Create with invalid date
1. Enter an invalid date format. 2. Click "Create".
Validation error displays for Enrollment Date.
Duplicate submission prevention (PRG)
1. Submit the form. 2. Refresh Index page.
Student is not duplicated. Success message appears only once.

2. Edit Student
Test Case
Steps
Expected Result
Edit existing student with valid data
1. Navigate to "Edit" page for a student. 2. Change fields. 3. Click "Save".
Changes are saved. Redirected to Index. Success message displays.
Edit with invalid data
1. Remove required field or enter invalid date. 2. Click "Save".
Validation errors display. No changes saved.
Edit non-existent student
1. Navigate to /Students/Edit?id=9999.
Page returns 404 Not Found.
Duplicate submission prevention (PRG)
1. Submit edits. 2. Refresh Index page.
Changes are not duplicated. Success message appears only once.

3. Navigation & UI Checks
Back to List links on Create and Edit pages return to Index.
Validation messages display correctly for all fields.
Success messages appear only after successful POST.
Instructions to Run
Open the project in Visual Studio or your preferred IDE.
Ensure the database is up to date using EF Core migrations.
Navigate to Students → Create or Students → Edit to test functionality.
Observe success messages and validation behavior.
Week 6 Revision – Course Enrollment
Data Seeding - XML Schema Note
Data seeding is performed by the DbInitializer class, which reads from the Data/SeedData.xml file on application startup.
Root Element: The root element for the entire data set is <School>.
Entity Group Elements: The data is organized into pluralized parent elements: <Students>, <Instructors>, <Departments>, <Courses>, <OfficeAssignments>, <CourseAssignments>, and <Enrollments>.
Record Elements: Each individual record is represented by a singular, lowercase element (e.g., <student>, <instructor>, <course>). This casing is important as the parser specifically looks for these lowercase names.
Key Fields: The XML includes explicit primary keys (e.g., <ID>, <CourseID>) for records. The DbInitializer uses these keys and temporarily enables IDENTITY_INSERT on the corresponding tables to ensure the data is inserted with the exact relationships specified in the file. This process is idempotent, meaning it will not create duplicate data on subsequent runs.
Console Log
The following is a short log from a successful run, showing the database creation and migration process.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (212ms) [Parameters=[], CommandType='Text', CommandTimeout='60']
      CREATE DATABASE [SchoolContext-ec5dab25-74b4-4d75-913c-ac3da2cf9c9d];
info: Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20250926075042_FinalInitial'.
info: Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20250926075403_FinalSchema'.
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7207
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5158
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\Joseph\ContosoUniversity


