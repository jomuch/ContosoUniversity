using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly SchoolContext _context;

        public EditModel(SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            // Load the department with administrator included (needed for display/edit)
            var department = await _context.Departments
                .Include(d => d.Administrator)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (department == null)
                return NotFound();

            Department = department;

            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var departmentToUpdate = await _context.Departments
                .Include(d => d.Administrator)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (departmentToUpdate == null)
            {
                // Department was deleted by another user
                var deletedDepartment = new Department();
                await TryUpdateModelAsync(deletedDepartment);
                ModelState.AddModelError(string.Empty,
                    "Unable to save. The department was deleted by another user.");
                return Page();
            }

            // Set the original RowVersion value to detect concurrency conflicts
            _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = Department.RowVersion;

            if (await TryUpdateModelAsync<Department>(
                departmentToUpdate,
                "Department",
                d => d.Name, d => d.StartDate, d => d.Budget, d => d.InstructorID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save. The department was deleted by another user.");
                        return Page();
                    }

                    var databaseValues = (Department)databaseEntry.ToObject();

                    // Show current database values in validation errors
                    if (databaseValues.Name != clientValues.Name)
                        ModelState.AddModelError("Department.Name", $"Current value: {databaseValues.Name}");
                    if (databaseValues.Budget != clientValues.Budget)
                        ModelState.AddModelError("Department.Budget", $"Current value: {databaseValues.Budget:c}");
                    if (databaseValues.StartDate != clientValues.StartDate)
                        ModelState.AddModelError("Department.StartDate", $"Current value: {databaseValues.StartDate:d}");
                    if (databaseValues.InstructorID != clientValues.InstructorID)
                    {
                        var databaseAdministrator = await _context.Instructors
                            .FirstOrDefaultAsync(i => i.ID == databaseValues.InstructorID);
                        ModelState.AddModelError("Department.InstructorID", $"Current value: {databaseAdministrator?.FullName}");
                    }

                    ModelState.AddModelError(string.Empty,
                        "The record you attempted to edit was modified by another user after you got the original values. " +
                        "The edit operation was canceled and the current values in the database have been displayed. " +
                        "If you still want to edit this record, click the Save button again.");

                    // Update the RowVersion to the new value for the form
                    Department.RowVersion = (byte[])databaseValues.RowVersion!;
                    ModelState.Remove("Department.RowVersion");
                }
            }

            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return Page();
        }
    }
}
