using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Repositories;
using StudentManagementSystem.Services;
using StudentManagementSystem.Models;
using StudentManagementSystem.DTOs;
using Microsoft.AspNetCore.Authorization;  // For AllowAnonymous


namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;

        public AuthController(IUserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        
        // [HttpPost("register")]
        // [AllowAnonymous]
        // public async Task<IActionResult> Register([FromBody] User user)
        // {
        //     if (user == null)
        //     {
        //         return BadRequest(new { message = "Invalid user data" });
        //     }

        //     var result = await _userRepository.RegisterUserAsync(user);
        //     if (!result.Success) return BadRequest(result.Message);

        //     return Ok(new { message = "User registered successfully" });
        // }

        // [HttpPost("login")]
        // public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        // {
        //     var token = await _authService.AuthenticateAsync(loginRequest);
        //     if (token == null)
        //     {
        //         return Unauthorized(new { message = "Invalid credentials" });
        //     }

        //     return Ok(new { token });
        // }
   
    }
}
