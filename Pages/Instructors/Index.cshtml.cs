using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public IndexModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        // Property is now initialized to prevent nullability warnings.
        public InstructorIndexData InstructorData { get; set; } = new();
        public int InstructorID { get; set; }
        public int CourseID { get; set; }

        public async Task OnGetAsync(int? id, int? courseID)
        {
            // This first query is already well-structured.
            InstructorData.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(c => c.Course)
                        .ThenInclude(c => c.Department)
                .OrderBy(i => i.LastName)
                .ToListAsync();

            if (id != null)
            {
                InstructorID = id.Value;
                // Using SingleOrDefault is safer than Single() and prevents crashes if no match is found.
                Instructor? instructor = InstructorData.Instructors
                    .Where(i => i.ID == id.Value).SingleOrDefault();

                if (instructor != null)
                {
                    InstructorData.Courses = instructor.CourseAssignments.Select(s => s.Course);
                }
            }

            if (courseID != null)
            {
                CourseID = courseID.Value;

                // This is the efficient way to load the enrollments and students.
                // It uses one database query instead of many.
                var selectedCourse = await _context.Courses
                    .Include(c => c.Enrollments)
                        .ThenInclude(e => e.Student)
                    .AsNoTracking() // Good practice for read-only data
                    .FirstOrDefaultAsync(c => c.CourseID == courseID.Value);

                if (selectedCourse != null)
                {
                    InstructorData.Enrollments = selectedCourse.Enrollments;
                }
            }
        }
    }
}