using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public CreateModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // This line populates the ViewData with a SelectList of Departments
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var emptyCourse = new Course();

            if (await TryUpdateModelAsync<Course>(
                emptyCourse,
                "course",
                s => s.CourseID, s => s.Credits, s => s.Title, s => s.DepartmentID))
            {
                _context.Courses.Add(emptyCourse);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // Reload the view with the correct department list if the model is not valid
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            return Page();
        }
    }
}