using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class GradeRepository
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
    public async Task<List<Grade>> GetGradesByTeacherIdAsync(int teacherId)
    {
        var courseIds = await _context.Courses
            .Where(c => c.TeacherId == teacherId)
            .Select(c => c.Id)
            .ToListAsync();

        if (courseIds.Count == 0)
        {
            return new List<Grade>();
        }

        return await _context.Grades
            .Where(g => courseIds.Contains(g.CourseId))
            .ToListAsync();
    }


}
