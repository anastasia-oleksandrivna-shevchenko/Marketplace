using Microsoft.AspNetCore.Identity;

namespace Marketplace.DAL.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Store> Stores { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Reviews { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}