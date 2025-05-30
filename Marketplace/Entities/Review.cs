namespace Marketplace.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Review
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReviewId { get; set; }

    [Required] public int ProductId { get; set; }
    [ForeignKey("ProductId")] public Product Product { get; set; }

    [Required] public int UserId { get; set; }
    [ForeignKey("UserId")] public User User { get; set; }

    [Required, Range(1, 5)] public int Rating { get; set; }

    [MaxLength(1000)] public string Comment { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}