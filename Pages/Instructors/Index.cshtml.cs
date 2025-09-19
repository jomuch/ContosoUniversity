using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public IndexModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public PaginatedList<Instructor> Instructor { get; set; }
        public string NameSort { get; set; } = null!;
        public string DateSort { get; set; } = null!;
        public string CurrentFilter { get; set; } = null!;
        public string CurrentSort { get; set; } = null!;
        public Instructor InstructorData { get; set; } = new Instructor();
        public ICollection<Course> AssignedCoursesData { get; set; } = new List<Course>();
        public int InstructorID { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, int? id)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;

            IQueryable<Instructor> instructorsIQ = from i in _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                                                   select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                instructorsIQ = instructorsIQ.Where(i => i.LastName.Contains(searchString) || i.FirstMidName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    instructorsIQ = instructorsIQ.OrderByDescending(i => i.LastName);
                    break;
                case "Date":
                    instructorsIQ = instructorsIQ.OrderBy(i => i.HireDate);
                    break;
                case "date_desc":
                    instructorsIQ = instructorsIQ.OrderByDescending(i => i.HireDate);
                    break;
                default:
                    instructorsIQ = instructorsIQ.OrderBy(i => i.LastName);
                    break;
            }

            int pageSize = 3;
            Instructor = await PaginatedList<Instructor>.CreateAsync(instructorsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            if (id != null)
            {
                InstructorID = id.Value;
                InstructorData = Instructor.Single(i => i.ID == id.Value);
                var courses = InstructorData.CourseAssignments.Select(s => s.Course);
                AssignedCoursesData = await courses.ToListAsync();
            }
        }
    }
}