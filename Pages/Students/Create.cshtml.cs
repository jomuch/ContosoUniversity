using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;

namespace ContosoUniversity.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly SchoolContext _context;

        public CreateModel(SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudentViewModel StudentVM { get; set; } = new StudentViewModel();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // redisplay with errors
            }

            var student = new Student
            {
                FirstMidName = StudentVM.FirstMidName,
                LastName = StudentVM.LastName,
                EnrollmentDate = StudentVM.EnrollmentDate
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Student created successfully!";
            return RedirectToPage("./Index"); // PRG pattern
        }
    }
}
