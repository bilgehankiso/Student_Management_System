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

    // Method to add a new course
    public async Task AddCourseAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }
}
