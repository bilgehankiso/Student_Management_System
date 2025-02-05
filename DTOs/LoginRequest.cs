namespace StudentManagementSystem.DTOs
{
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;  // `Username` yerine `Email` kullan
    public string Password { get; set; } = string.Empty;
}

}
