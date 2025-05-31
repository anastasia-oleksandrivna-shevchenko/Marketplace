namespace Marketplace.BBL.DTO.User;

public class ChangeEmailDto
{
    public int UserId { get; set; }
    public string NewEmail { get; set; }
    public string Password { get; set; }
}