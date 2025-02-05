using StudentManagementSystem.Models;
using StudentManagementSystem.DTOs;
using System.Threading.Tasks;

namespace StudentManagementSystem.Repositories
{
    public interface IUserRepository
    {
        Task<ServiceResult<User>> RegisterUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
