using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;  // For Course
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;  // For AllowAnonymous


namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseRepository _courseRepository;

        public CourseController(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        // POST api/course/add
        [HttpPost("add")]
        [AllowAnonymous]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            if (course == null)
            {
                return BadRequest("Invalid course data.");
            }

            await _courseRepository.AddCourseAsync(course);
            return Ok("Course added successfully.");
        }



    }
}
