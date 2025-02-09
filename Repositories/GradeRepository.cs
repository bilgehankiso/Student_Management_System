using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using StudentManagementSystem.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Repositories
{
    public class GradeRepository: IGradeRepository
    {
        private readonly ApplicationDbContext _context;

        public GradeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrUpdateGradeAsync(Grade grade)
        {
            var existingGrade = await _context.Grades
                .FirstOrDefaultAsync(g => g.StudentId == grade.StudentId && g.CourseId == grade.CourseId);

            if (existingGrade == null)
            {
                await _context.Grades.AddAsync(grade);
            }
            else
            {
                existingGrade.Midterm = grade.Midterm;
                existingGrade.Final = grade.Final;
                _context.Grades.Update(existingGrade);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Grade?> GetGradeAsync(int studentId, int courseId)
        {
            return await _context.Grades
                .FirstOrDefaultAsync(g => g.StudentId == studentId && g.CourseId == courseId);
        }

        public async Task<List<Grade>> GetGradesByStudentIdAsync(int studentId)
        {
            return await _context.Grades
                .Where(g => g.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<GradeTeacherDTO>> GetGradesByTeacherIdAsync(int teacherId)
        {
            return await (from g in _context.Grades
                          join s in _context.Users on g.StudentId equals s.Id
                          join c in _context.Courses on g.CourseId equals c.Id
                          where c.TeacherId == teacherId
                          select new GradeTeacherDTO
                          {
                              Id = g.Id,
                              StudentId = s.Id,
                              StudentName = s.Name,
                              CourseId = c.Id,
                              CourseName = c.Name,
                              Midterm = g.Midterm,
                              Final = g.Final
                          }).ToListAsync();
        }
    }
}