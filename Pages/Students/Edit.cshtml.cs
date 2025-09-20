using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly SchoolContext _context;

        public EditModel(SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student? Student { get; set; }

        // GET: Load student to edit
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Student = await _context.Students.FindAsync(id);

            if (Student == null)
                return NotFound();

            return Page();
        }

        // POST: Save edits
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var studentToUpdate = await _context.Students.FindAsync(id);

            if (studentToUpdate == null)
                return NotFound();

            // Try updating the model
            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "student",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                await _context.SaveChangesAsync();

                // TempData message for PRG
                TempData["SuccessMessage"] = "Student updated successfully!";

                // PRG: redirect after POST
                return RedirectToPage("./Index");
            }

            // If validation fails, stay on the page
            return Page();
        }
    }
}
