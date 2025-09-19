using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
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

        // Property for dropdown list of departments
        public SelectList DepartmentList { get; set; } = default!;

        // Bind property for the course to create; initialized to avoid null warnings
        [BindProperty]
        public Course Course { get; set; } = new Course();

        public IActionResult OnGet()
        {
            // Populate dropdown list
            DepartmentList = new SelectList(_context.Departments, "DepartmentID", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // If model validation fails, reload dropdown and return page
                DepartmentList = new SelectList(_context.Departments, "DepartmentID", "Name");
                return Page();
            }

            _context.Courses.Add(Course);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
