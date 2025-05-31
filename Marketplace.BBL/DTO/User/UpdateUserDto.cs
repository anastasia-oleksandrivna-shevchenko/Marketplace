namespace Marketplace.BBL.DTO.User;

public class UpdateUserDto
{
    public int UserId { get; set; }

    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Phone { get; set; }
    
}