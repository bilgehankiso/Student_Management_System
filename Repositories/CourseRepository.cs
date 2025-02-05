using StudentManagementSystem.Models;  // For Course
using Microsoft.EntityFrameworkCore;  // For ApplicationDbContext
using System.Threading.Tasks;
using StudentManagementSystem.Data;  // For ApplicationDbContext

public class CourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddCourseAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteCourseAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
    }

}
