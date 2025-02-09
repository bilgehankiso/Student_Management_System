using StudentManagementSystem.Models;
using StudentManagementSystem.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public interface IGradeService
    {
        Task<bool> AddOrUpdateGradeAsync(Grade grade);
        Task<Grade?> GetGradeAsync(int studentId, int courseId);
        Task<List<Grade>> GetGradesByStudentIdAsync(int studentId);
        Task<List<GradeTeacherDTO>> GetGradesByTeacherIdAsync(int teacherId);
    }
}
