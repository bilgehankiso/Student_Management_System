using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Services;
using System.Threading.Tasks;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userService.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return Conflict("This email address is already registered");
            }

            var result = await _userService.RegisterUserAsync(user);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { message = "Invalid login data" });
            }

            var user = await _userService.GetUserByEmailAsync(loginDto.Email);
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
            var users = await _userService.GetUsersByRoleAsync(role);

            if (users == null || !users.Any())
            {
                return NotFound("No " + role + " found.");
            }

            var userList = users.Select(t => new RoleDto
            {
                Id = t.Id,
                Name = t.Name,
                Role = t.Role
            });
            return Ok(userList);
        }
    }
}
