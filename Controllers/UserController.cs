using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Repositories;
using StudentManagementSystem.DTOs;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return Conflict("Bu email adresi zaten kayıtlı.");
            }

            var result = await _userRepository.RegisterUserAsync(user);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { message = "Invalid login data" });
            }

            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            if (user.PasswordHash != loginDto.Password)
            {
                return Unauthorized(new { message = "Invalid password" });
            }

            return Ok(new
            {
                message = "Login successful",
                success = true,
                user = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                }
            });
        }

        [HttpGet("getAllUsersByRole")]
        public async Task<IActionResult> GetAllUsersByRole(string role)
        {
            var teachers = await _userRepository.GetUsersByRoleAsync(role);

            if (teachers == null || !teachers.Any())
            {
                return NotFound("No" + role + "found.");
            }

            var teacherList = teachers.Select(t => new RoleDto
            {
                Id = t.Id,
                Name = t.Name,
                Role = t.Role
            });
            return Ok(teacherList);
        }
    }
}
