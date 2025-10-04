# Contoso University - Week 7 Submission

## Repository & Test Files

* **Repository Link**: https://github.com/jomuch/ContosoUniversity.git
* **Unit Test File**: The unit tests for the DbExportService can be found in /ContosoUniversity.Tests/DbExportServiceTests.cs.
* **Integration Test File**: The integration test for the Students Index page can be found in /ContosoUniversity.Tests/StudentPagesTests.cs.

## Structured Logging

Structured logging was added to the OnGetAsync method of the Details.cshtml.cs PageModel for the Students section. This provides clear, queryable information whenever a user views a student's details.

### Sample Log Output

The following is a sample log output captured from the debug console when navigating to the details page for the student with ID = 1. The StudentId is captured as a distinct property, which is invaluable for filtering and diagnostics in a real-world logging system like Seq or Application Insights.

`
info: ContosoUniversity.Pages.Students.DetailsModel[0]
      Viewing details for Student ID: 1
`

A warning is also logged if a non-existent student ID is requested:

`
warn: ContosoUniversity.Pages.Students.DetailsModel[0]
      Student with ID: 9999 not found.
`
