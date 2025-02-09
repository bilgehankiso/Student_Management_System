using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Services;


namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // POST api/course/addorupdate
        [HttpPost("addorupdate")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdateCourse([FromBody] Course course)
        {
            if (course == null)
            {
                return BadRequest("Invalid course data");
            }

            if (course.Id > 0)
            {
                var existingCourse = await _courseService.GetCourseByIdAsync(course.Id);
                if (existingCourse == null)
                {
                    return NotFound("Course not found");
                }

                existingCourse.Name = course.Name;
                existingCourse.TeacherId = course.TeacherId;

                await _courseService.UpdateCourseAsync(existingCourse);
                return Ok("Course updated successfully");
            }
            else
            {
                await _courseService.AddCourseAsync(course);
                return Ok("Course added successfully");
            }
        }

        [HttpDelete("delete/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            await _courseService.DeleteCourseAsync(id);
            return Ok(new { message = "Course deleted successfully" });
        }

        [HttpPost("join")]
        [AllowAnonymous]
        public async Task<IActionResult> JoinCourse([FromBody] StudentCourse studentCourse)
        {
            if (studentCourse == null || studentCourse.StudentId <= 0 || studentCourse.CourseId <= 0)
            {
                return BadRequest(new { message = "Invalid student or course data" });
            }

            var success = await _courseService.JoinCourseAsync(studentCourse.StudentId, studentCourse.CourseId);

            if (!success)
            {
                return BadRequest(new { message = "Failed to join course. The course may not exist or the student may already be enrolled" });
            }

            return Ok(new { message = "Student successfully joined the course" });
        }

        [HttpPost("leave")]
        [AllowAnonymous]
        public async Task<IActionResult> LeaveCourse([FromBody] StudentCourse studentCourse)
        {
            if (studentCourse == null || studentCourse.StudentId <= 0 || studentCourse.CourseId <= 0)
            {
                return BadRequest(new { message = "Invalid student or course data" });
            }

            var success = await _courseService.LeaveCourseAsync(studentCourse.StudentId, studentCourse.CourseId);

            if (!success)
            {
                return BadRequest(new { message = "Failed to leave course. The student may not be enrolled in this course" });
            }

            return Ok(new { message = "Student successfully left the course" });
        }

        [HttpGet("teacher/{teacherId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCoursesByTeacher(int teacherId)
        {
            var courses = await _courseService.GetCoursesByTeacherAsync(teacherId);
            if (courses == null || !courses.Any())
            {
                return NotFound(new { message = "No courses found for this teacher" });
            }

            return Ok(courses);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCourses()
        {
            var coursesWithTeachers = await _courseService.GetAllCoursesWithTeacherAsync();
            if (coursesWithTeachers == null || !coursesWithTeachers.Any())
            {
                return NotFound(new { message = "No courses found" });
            }

            return Ok(coursesWithTeachers);
        }

        [HttpGet("{courseId}/students")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStudentsByCourseId(int courseId)
        {
            var students = await _courseService.GetStudentsByCourseIdAsync(courseId);
            if (students == null || students.Count == 0)
            {
                return NotFound(new { message = "No students found for this course" });
            }

            return Ok(students);
        }
    }
}
