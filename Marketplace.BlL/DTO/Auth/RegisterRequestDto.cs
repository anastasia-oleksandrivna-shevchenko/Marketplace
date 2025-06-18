namespace Marketplace.BLL.DTO.Auth;

public class RegisterRequestDto
{
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; } 
    public string? Role { get; set; }
}