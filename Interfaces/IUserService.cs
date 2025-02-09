using StudentManagementSystem.Models;
using StudentManagementSystem.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public interface IUserService
    {
        Task<ServiceResult<User>> RegisterUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetUsersByRoleAsync(string role);
    }
}
