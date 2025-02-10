using StudentManagementSystem.Models;
using StudentManagementSystem.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message, User? User)> RegisterUserAsync(User user);
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<(bool Success, string Message, User? User)> LoginUserAsync(LoginRequest loginDto);

    }
}
