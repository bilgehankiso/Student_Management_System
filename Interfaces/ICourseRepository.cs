using StudentManagementSystem.Models;
using StudentManagementSystem.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementSystem.Repositories
{
    public interface ICourseRepository
    {
        Task AddCourseAsync(Course course);
        Task DeleteCourseAsync(int id);
        Task<Course?> GetCourseByIdAsync(int id);
        Task<bool> JoinCourseAsync(int studentId, int courseId);
        Task<bool> LeaveCourseAsync(int studentId, int courseId);
        Task<List<Course>> GetCoursesByTeacherAsync(int teacherId);
        Task<List<CourseWithTeacher>> GetAllCoursesWithTeacherAsync();
        Task UpdateCourseAsync(Course course);
        Task<List<StudentDTO>> GetStudentsByCourseIdAsync(int courseId);
    }
}
