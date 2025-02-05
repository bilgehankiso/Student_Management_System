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
        
        [HttpDelete("delete/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            await _courseRepository.DeleteCourseAsync(id);
            return Ok(new { message = "Course deleted successfully" });
        }

    }
}
