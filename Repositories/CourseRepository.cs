using StudentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using StudentManagementSystem.Data;
using StudentManagementSystem.DTOs;

namespace StudentManagementSystem.Repositories
{
    public class CourseRepository : ICourseRepository
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
        public async Task<bool> JoinCourseAsync(int studentId, int courseId)
        {
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == courseId);
            if (!courseExists)
            {
                return false;
            }

            var alreadyJoined = await _context.StudentCourses
                .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (alreadyJoined)
            {
                return false;
            }

            var studentCourse = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId
            };

            await _context.StudentCourses.AddAsync(studentCourse);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> LeaveCourseAsync(int studentId, int courseId)
        {
            var studentCourse = await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (studentCourse == null)
            {
                return false;
            }

            _context.StudentCourses.Remove(studentCourse);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<Course>> GetCoursesByTeacherAsync(int teacherId)
        {
            return await _context.Courses
                                 .Where(c => c.TeacherId == teacherId)
                                 .ToListAsync();
        }
        public async Task<List<CourseWithTeacher>> GetAllCoursesWithTeacherAsync()
        {
            var coursesWithTeachers = await _context.Courses
                .Join(_context.Users,
                      course => course.TeacherId,
                      user => user.Id,
                      (course, user) => new CourseWithTeacher
                      {
                          Id = course.Id,
                          Name = course.Name,
                          TeacherId = course.TeacherId,
                          TeacherName = user.Name
                      })
                .ToListAsync();

            return coursesWithTeachers;
        }
        public async Task UpdateCourseAsync(Course course)
        {
            var existingCourse = await _context.Courses.FindAsync(course.Id);
            if (existingCourse != null)
            {
                existingCourse.Name = course.Name;
                existingCourse.TeacherId = course.TeacherId;

                _context.Courses.Update(existingCourse);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<StudentDTO>> GetStudentsByCourseIdAsync(int courseId)
        {
            return await (from sc in _context.StudentCourses
                          join u in _context.Users on sc.StudentId equals u.Id
                          where sc.CourseId == courseId
                          select new StudentDTO
                          {
                              Id = u.Id,
                              Name = u.Name,
                              CourseId = sc.CourseId
                          }).ToListAsync();
        }


    }
}