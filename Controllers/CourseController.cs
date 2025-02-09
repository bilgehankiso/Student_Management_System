using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Repositories;



namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        // POST api/course/addorupdate
        [HttpPost("addorupdate")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdateCourse([FromBody] Course course)
        {
            if (course == null)
            {
                return BadRequest("Invalid course data.");
            }

            if (course.Id > 0) // ID varsa, Güncelleme işlemi yap
            {
                var existingCourse = await _courseRepository.GetCourseByIdAsync(course.Id);
                if (existingCourse == null)
                {
                    return NotFound("Course not found.");
                }

                existingCourse.Name = course.Name;
                existingCourse.TeacherId = course.TeacherId;

                await _courseRepository.UpdateCourseAsync(existingCourse);
                return Ok("Course updated successfully.");
            }
            else // ID yoksa, Yeni ekleme işlemi yap
            {
                await _courseRepository.AddCourseAsync(course);
                return Ok("Course added successfully.");
            }
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

        [HttpPost("join")]
        [AllowAnonymous]
        public async Task<IActionResult> JoinCourse([FromBody] StudentCourse studentCourse)
        {
            if (studentCourse == null || studentCourse.StudentId <= 0 || studentCourse.CourseId <= 0)
            {
                return BadRequest(new { message = "Invalid student or course data" });
            }

            var success = await _courseRepository.JoinCourseAsync(studentCourse.StudentId, studentCourse.CourseId);

            if (!success)
            {
                return BadRequest(new { message = "Failed to join course. The course may not exist or the student may already be enrolled." });
            }

            return Ok(new { message = "Student successfully joined the course." });
        }

        [HttpPost("leave")]
        [AllowAnonymous]
        public async Task<IActionResult> LeaveCourse([FromBody] StudentCourse studentCourse)
        {
            if (studentCourse == null || studentCourse.StudentId <= 0 || studentCourse.CourseId <= 0)
            {
                return BadRequest(new { message = "Invalid student or course data" });
            }

            var success = await _courseRepository.LeaveCourseAsync(studentCourse.StudentId, studentCourse.CourseId);

            if (!success)
            {
                return BadRequest(new { message = "Failed to leave course. The student may not be enrolled in this course." });
            }

            return Ok(new { message = "Student successfully left the course." });
        }

        [HttpGet("teacher/{teacherId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCoursesByTeacher(int teacherId)
        {
            var courses = await _courseRepository.GetCoursesByTeacherAsync(teacherId);
            if (courses == null || !courses.Any())
            {
                return NotFound(new { message = "No courses found for this teacher." });
            }

            return Ok(courses);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCourses()
        {
            var coursesWithTeachers = await _courseRepository.GetAllCoursesWithTeacherAsync();
            if (coursesWithTeachers == null || !coursesWithTeachers.Any())
            {
                return NotFound(new { message = "No courses found." });
            }

            return Ok(coursesWithTeachers);
        }
        [HttpGet("{courseId}/students")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStudentsByCourseId(int courseId)
        {
            var students = await _courseRepository.GetStudentsByCourseIdAsync(courseId);
            if (students == null || students.Count == 0)
            {
                return NotFound(new { message = "No students found for this course." });
            }

            return Ok(students);
        }

    }
}
