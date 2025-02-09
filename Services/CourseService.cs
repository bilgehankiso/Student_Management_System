using StudentManagementSystem.Models;
using StudentManagementSystem.Repositories;
using StudentManagementSystem.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task AddCourseAsync(Course course)
        {
            await _courseRepository.AddCourseAsync(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            await _courseRepository.DeleteCourseAsync(id);
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetCourseByIdAsync(id);
        }

        public async Task<bool> JoinCourseAsync(int studentId, int courseId)
        {
            return await _courseRepository.JoinCourseAsync(studentId, courseId);
        }

        public async Task<bool> LeaveCourseAsync(int studentId, int courseId)
        {
            return await _courseRepository.LeaveCourseAsync(studentId, courseId);
        }

        public async Task<List<Course>> GetCoursesByTeacherAsync(int teacherId)
        {
            return await _courseRepository.GetCoursesByTeacherAsync(teacherId);
        }

        public async Task<List<CourseWithTeacher>> GetAllCoursesWithTeacherAsync()
        {
            return await _courseRepository.GetAllCoursesWithTeacherAsync();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            await _courseRepository.UpdateCourseAsync(course);
        }

        public async Task<List<StudentDTO>> GetStudentsByCourseIdAsync(int courseId)
        {
            return await _courseRepository.GetStudentsByCourseIdAsync(courseId);
        }
    }
}
