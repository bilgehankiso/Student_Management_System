using StudentManagementSystem.Models;

namespace StudentManagementSystem.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task AddUser(User user);
    }
}
