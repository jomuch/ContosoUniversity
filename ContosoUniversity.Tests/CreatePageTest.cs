using ContosoUniversity.Data;
using ContosoUniversity.Models.ViewModels;
using ContosoUniversity.Pages.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace ContosoUniversity.Tests
{
    public class CreatePageTests
    {
        [Fact]
        public async Task OnPostAsync_ReturnsARedirectToPageResult_WhenModelStateIsValid()
        {
            var options = new DbContextOptionsBuilder<SchoolContext>()
                .UseInMemoryDatabase(databaseName: "Test_CreateValidVM")
                .Options;

            using (var context = new SchoolContext(options))
            {
                var pageModel = new CreateModel(context);
                pageModel.StudentVM = new StudentViewModel
                {
                    FirstMidName = "Jane",
                    LastName = "Doe",
                    EnrollmentDate = System.DateTime.Now
                };

                var result = await pageModel.OnPostAsync();

                Assert.IsType<RedirectToPageResult>(result);
            }
        }

        [Fact]
        public async Task OnPostAsync_ReturnsAPageResult_WhenModelStateIsInvalid()
        {
            var options = new DbContextOptionsBuilder<SchoolContext>()
                .UseInMemoryDatabase(databaseName: "Test_CreateInvalidVM")
                .Options;

            using (var context = new SchoolContext(options))
            {
                var pageModel = new CreateModel(context);
                pageModel.StudentVM = new StudentViewModel();
                pageModel.ModelState.AddModelError("StudentVM.LastName", "Required");

                var result = await pageModel.OnPostAsync();

                Assert.IsType<PageResult>(result);
            }
        }
    }
}