using StudentManagementSystem.Models;
using StudentManagementSystem.Repositories;
using StudentManagementSystem.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;

        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        public async Task<bool> AddOrUpdateGradeAsync(Grade grade)
        {
            if (grade.Midterm < 0 || grade.Midterm > 100 || grade.Final < 0 || grade.Final > 100)
            {
                return false;
            }

            return await _gradeRepository.AddOrUpdateGradeAsync(grade);
        }


        public async Task<Grade?> GetGradeAsync(int studentId, int courseId)
        {
            return await _gradeRepository.GetGradeAsync(studentId, courseId);
        }

        public async Task<List<Grade>> GetGradesByStudentIdAsync(int studentId)
        {
            return await _gradeRepository.GetGradesByStudentIdAsync(studentId);
        }

        public async Task<List<GradeTeacherDTO>> GetGradesByTeacherIdAsync(int teacherId)
        {
            return await _gradeRepository.GetGradesByTeacherIdAsync(teacherId);
        }
    }
}
