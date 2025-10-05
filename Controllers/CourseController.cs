using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContosoUniversity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            _logger.LogInformation("Fetching course with id {CourseID}", id);
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                _logger.LogWarning("Course with id {CourseID} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Returning course {@Course}", new { course.CourseID, course.Title, course.Department });
            return Ok(course);
        }
    }
}
