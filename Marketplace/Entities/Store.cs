namespace Marketplace.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Store
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StoreId { get; set; }

    [Required] public int UserId { get; set; }
    [ForeignKey("UserId")] public User User { get; set; }

    [Required] [MaxLength(100)] public string StoreName { get; set; }

    [MaxLength(1000)] public String Description { get; set; }

    [MaxLength(200)] public string Location { get; set; }

    [Range(0, 5)] public float Rating { get; set; }
    
    public ICollection<Product> Products { get; set; }
    public ICollection<Order> Orders { get; set; }
}

