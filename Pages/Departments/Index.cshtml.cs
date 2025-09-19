using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public IndexModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<Department> Department { get; set; } = default!;
        public string NameSort { get; set; } = null!;
        public string BudgetSort { get; set; } = null!;
        public string StartDateSort { get; set; } = null!;
        public string InstructorSort { get; set; } = null!;

        public async Task OnGetAsync(string sortOrder)
        {
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            BudgetSort = sortOrder == "Budget" ? "budget_desc" : "Budget";
            StartDateSort = sortOrder == "StartDate" ? "startdate_desc" : "StartDate";
            InstructorSort = sortOrder == "Instructor" ? "instructor_desc" : "Instructor";

            IQueryable<Department> departments = _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking();

            switch (sortOrder)
            {
                case "name_desc":
                    departments = departments.OrderByDescending(d => d.Name);
                    break;
                case "Budget":
                    departments = departments.OrderBy(d => d.Budget);
                    break;
                case "budget_desc":
                    departments = departments.OrderByDescending(d => d.Budget);
                    break;
                case "StartDate":
                    departments = departments.OrderBy(d => d.StartDate);
                    break;
                case "startdate_desc":
                    departments = departments.OrderByDescending(d => d.StartDate);
                    break;
                case "Instructor":
                    departments = departments.OrderBy(d => d.Administrator == null ? "" : d.Administrator.LastName);
                    break;
                case "instructor_desc":
                    departments = departments.OrderByDescending(d => d.Administrator == null ? "" : d.Administrator.LastName);
                    break;
                default:
                    departments = departments.OrderBy(d => d.Name);
                    break;
            }

            Department = await departments.ToListAsync();
        }
    }
}