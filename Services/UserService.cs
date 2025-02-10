using StudentManagementSystem.Models;
using StudentManagementSystem.Repositories;
using StudentManagementSystem.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StudentManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string Message, User? User)> RegisterUserAsync(User user)
        {
            if (user == null ||
                string.IsNullOrEmpty(user.Email) ||
                string.IsNullOrEmpty(user.PasswordHash) ||
                string.IsNullOrEmpty(user.Role) ||
                string.IsNullOrEmpty(user.Name))
            {
                return (false, "Please check E-Mail, Password, Name, and Role", null);
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return (false, "This email address is already registered", null);
            }

            var result = await _userRepository.RegisterUserAsync(user);

            if (result == null || !result.Success)
            {
                return (false, "Registration failed", null);
            }

            return (true, "Successfully registered", result.Data);
        }

        private async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            return await _userRepository.GetUsersByRoleAsync(role);
        }

        public async Task<(bool Success, string Message, User? User)> LoginUserAsync(LoginRequest loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return (false, "Please check E-Mail and Password", null);
            }

            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return (false, "User not found", null);
            }

            if (user.PasswordHash != loginDto.Password)
            {
                return (false, "Invalid password", null);
            }
            
            user.PasswordHash = null;
            return (true, "Login successful", user);
        }
    }
}
