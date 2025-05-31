namespace Marketplace.BBL.DTO.User;

public class ChangeUsernameDto
{
    public int UserId { get; set; }
    public string NewUsername { get; set; }
    public string Password { get; set; }
}