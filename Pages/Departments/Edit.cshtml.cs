using System;
using System.Collections.Generic;
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
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // The AsNoTracking() call is removed here because it's needed for concurrency handling
            var department = await _context.Departments
                .Include(d => d.Administrator)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (department == null)
            {
                return NotFound();
            }
            Department = department;
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentToUpdate = await _context.Departments
                .Include(i => i.Administrator)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (departmentToUpdate == null)
            {
                // Concurrency error has occurred.
                // The record was deleted by another user.
                var deletedDepartment = new Department();
                await TryUpdateModelAsync(deletedDepartment);
                ModelState.AddModelError(string.Empty,
                    "Unable to save. The department was deleted by another user.");
                return Page();
            }

            // Update the Department object with values from the form.
            _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = Department.RowVersion;

            if (await TryUpdateModelAsync<Department>(
                departmentToUpdate,
                "Department",
                s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Concurrency conflict occurred.
                    // This is the logic to handle it gracefully.
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save. " +
                            "The department was deleted by another user.");
                        return Page();
                    }

                    var databaseValues = (Department)databaseEntry.ToObject();
                    if (databaseValues.Name != clientValues.Name)
                    {
                        ModelState.AddModelError("Department.Name", $"Current value: {databaseValues.Name}");
                    }
                    if (databaseValues.Budget != clientValues.Budget)
                    {
                        ModelState.AddModelError("Department.Budget", $"Current value: {databaseValues.Budget:c}");
                    }
                    if (databaseValues.StartDate != clientValues.StartDate)
                    {
                        ModelState.AddModelError("Department.StartDate", $"Current value: {databaseValues.StartDate:d}");
                    }
                    if (databaseValues.InstructorID != clientValues.InstructorID)
                    {
                        Instructor databaseAdministrator = await _context.Instructors
                            .FirstOrDefaultAsync(i => i.ID == databaseValues.InstructorID);
                        ModelState.AddModelError("Department.InstructorID", $"Current value: {databaseAdministrator?.FullName}");
                    }

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. "
                            + "The edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again.");

                    // Reload the view with the new database values for the user to see
                    Department.RowVersion = (byte[])databaseValues.RowVersion;
                    ModelState.Remove("Department.RowVersion");
                }
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return Page();
        }
    }
}