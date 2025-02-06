using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly GradeRepository _gradeRepository;

        public GradeController(GradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        // POST api/grade/addOrUpdate
        [HttpPost("addOrUpdate")]
        [AllowAnonymous]
        //  [Authorize(Roles = "Teacher")] 
        public async Task<IActionResult> AddOrUpdateGrade([FromBody] Grade grade)
        {
            if (grade == null || grade.StudentId <= 0 || grade.CourseId <= 0)
            {
                return BadRequest(new { message = "Invalid grade data." });
            }

            var success = await _gradeRepository.AddOrUpdateGradeAsync(grade);

            if (!success)
            {
                return BadRequest(new { message = "Failed to add or update grade." });
            }

            return Ok(new { message = "Grade successfully added or updated." });
        }

        // GET api/grade/get/{studentId}/{courseId}
        [HttpGet("get/{studentId}/{courseId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGrade(int studentId, int courseId)
        {
            var grade = await _gradeRepository.GetGradeAsync(studentId, courseId);

            if (grade == null)
            {
                return NotFound(new { message = "Grade not found for this student and course." });
            }

            return Ok(grade);
        }
        // GET: api/grade/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetGradesByStudent(int studentId)
        {
            var grades = await _gradeRepository.GetGradesByStudentIdAsync(studentId);

            if (grades == null || grades.Count == 0)
            {
                return NotFound(new { message = "No grades found for this student" });
            }

            return Ok(grades);
        }
    }
}
