using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity.Utilities
{
    public static class PageNav
    {
        public static string? ActivePage(ViewContext viewContext)
        {
            var page = viewContext.View.Path;
            if (page.Contains("/Students/")) return "Students";
            if (page.Contains("/Courses/")) return "Courses";
            if (page.Contains("/Instructors/")) return "Instructors";
            if (page.Contains("/Departments/")) return "Departments";
            if (page.Contains("/About")) return "About";
            // Add other pages as needed
            return null;
        }
    }
}