using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        // POST api/grade/addOrUpdate
        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdateGrade([FromBody] Grade grade)
        {
            if (grade == null || grade.StudentId <= 0 || grade.CourseId <= 0)
            {
                return BadRequest(new { message = "Invalid grade data" });
            }

            var success = await _gradeService.AddOrUpdateGradeAsync(grade);

            if (!success)
            {
                return BadRequest(new { message = "Grades must be between 0 and 100" });
            }

            return Ok(new { message = "Grade successfully added or updated" });
        }


        // GET api/grade/get/{studentId}/{courseId}
        [HttpGet("get/{studentId}/{courseId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGrade(int studentId, int courseId)
        {
            var grade = await _gradeService.GetGradeAsync(studentId, courseId);

            if (grade == null)
            {
                return NotFound(new { message = "Grade not found for this student and course" });
            }

            return Ok(grade);
        }

        // GET: api/grade/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetGradesByStudent(int studentId)
        {
            var grades = await _gradeService.GetGradesByStudentIdAsync(studentId);

            if (grades == null || grades.Count == 0)
            {
                return NotFound(new { message = "No grades found for this student" });
            }

            return Ok(grades);
        }

        // GET api/grade/teacher/{teacherId}
        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<List<GradeTeacherDTO>>> GetGradesByTeacher(int teacherId)
        {
            var grades = await _gradeService.GetGradesByTeacherIdAsync(teacherId);

            if (grades == null || grades.Count == 0)
            {
                return NotFound(new { message = "No grades found for this teacher" });
            }

            return Ok(grades);
        }
    }
}
