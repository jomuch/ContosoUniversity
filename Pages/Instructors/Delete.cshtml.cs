using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Instructors
{
    public class DeleteModel : PageModel
    {
        private readonly SchoolContext _context;

        public DeleteModel(SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Instructor Instructor { get; set; } = default!;

        // holds the error message if delete fails
        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            Instructor = await _context.Instructors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Instructor == null)
                return NotFound();

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. The instructor may be assigned as administrator of a department.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
                return NotFound();

            try
            {
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException)
            {
                // Redirect back to page with error flag
                return RedirectToAction("./Delete", new { id, saveChangesError = true });
            }
        }
    }
}
