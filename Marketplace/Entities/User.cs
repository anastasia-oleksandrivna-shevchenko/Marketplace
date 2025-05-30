namespace Marketplace.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    [Required, MaxLength(50)] public string FirstName { get; set; }

    [Required, MaxLength(50)] public string LastName { get; set; }

    [MaxLength(50)] public string? MiddleName { get; set; }

    [Required, MaxLength(50)] public string Username { get; set; }

    [Required, MaxLength(100), EmailAddress]
    public string Email { get; set; }
    
    [Required, Phone]
    public string Phone { get; set; }

    [Required, MinLength(8, ErrorMessage = "The password must be at least 8 characters long."), DataType(DataType.Password)]
    public String PasswordHash { get; set; }

    [Required, RegularExpression("Buyer|Seller")]
    public string Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}