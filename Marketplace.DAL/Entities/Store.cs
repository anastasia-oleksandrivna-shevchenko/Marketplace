namespace Marketplace.DAL.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Store
{
    public int StoreId { get; set; }
    public int UserId { get; set; } 
    public User User { get; set; }
    public string StoreName { get; set; }
    public String Description { get; set; }
    public string Location { get; set; }
    public float Rating { get; set; }
    
    public ICollection<Product> Products { get; set; }
    public ICollection<Order> Orders { get; set; }
}

