using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly SchoolContext _context;

        public CreateModel(SchoolContext context)
        {
            _context = context;
        }

        // Bind the Course model to the form input
        [BindProperty]
        public Course Course { get; set; } = new Course();

        // Property for the department dropdown
        public SelectList DepartmentList { get; set; } = default!;

        // On GET: Populate the department list for the dropdown
        public IActionResult OnGet()
        {
            DepartmentList = new SelectList(_context.Departments, "DepartmentID", "Name");
            return Page();
        }

        // On POST: Save the new course to the database
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                DepartmentList = new SelectList(_context.Departments, "DepartmentID", "Name");
                return Page();
            }

            _context.Courses.Add(Course);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
