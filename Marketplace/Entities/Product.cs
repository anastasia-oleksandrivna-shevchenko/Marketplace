namespace Marketplace.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }

    [Required] public int StoreId { get; set; }
    [ForeignKey("StoreId")] public Store Store { get; set; }

    [Required, MaxLength(100)] public string Name { get; set; }

    [MaxLength(1000)] public string Description { get; set; }

    [Required, Column(TypeName = "decimal(11,2)"), DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required] public int Quantity { get; set; }

    [Required] public int CategoryId { get; set; }
    [ForeignKey("CategoryId")] public Category Category { get; set; }

    [Url]
    [MaxLength(500)] public string ImageUrl { get; set; }
}