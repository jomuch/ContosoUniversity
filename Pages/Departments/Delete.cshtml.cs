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
    public class DeleteModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public DeleteModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;
        public string ConcurrencyError { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            Department = await _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (Department == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return NotFound();
                }
                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ConcurrencyError = "The record you attempted to delete was modified by another user after you got the original values. "
                                   + "The delete operation was canceled. If you still want to delete this record, click the Delete button again.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                if (await _context.Departments.AnyAsync(m => m.DepartmentID == id))
                {
                    var departmentToDelete = new Department();
                    departmentToDelete.DepartmentID = id;
                    _context.Entry(departmentToDelete).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToPage("./Delete", new { concurrencyError = true, id = id });
            }
        }
    }
}