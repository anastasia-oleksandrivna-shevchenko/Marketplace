namespace Marketplace.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public int UserId { get; set; } 
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public String PasswordHash { get; set; }
    public string Role { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Store> Stores { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Reviews { get; set; }
}