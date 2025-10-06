using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly SchoolContext _context;

        public DeleteModel(SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            Student = await _context.Students
                .Include(s => s.Enrollments) // load related enrollments
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
                return NotFound();

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. The student may still have enrollments assigned.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .Include(s => s.Enrollments) // include related data
                .FirstOrDefaultAsync(s => s.ID == id);

            if (student == null)
                return NotFound();

            try
            {
                if (student.Enrollments.Count > 0)
                {
                    // prevent deletion if there are enrollments
                    ErrorMessage = "Cannot delete student with enrollments assigned.";
                    return Page();
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException)
            {
                // redirect back to Delete page with error
                return RedirectToPage("./Delete", new { id, saveChangesError = true });
            }
        }
    }
}
