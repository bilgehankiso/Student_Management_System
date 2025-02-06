using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Threading.Tasks;
using StudentManagementSystem.DTOs;

namespace StudentManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<User>> RegisterUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return ServiceResult<User>.SuccessResult(user, "Kullanıcı başarıyla kaydedildi.");
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            return await _context.Users.Where(u => u.Role == role).ToListAsync();
        }
    }
}
