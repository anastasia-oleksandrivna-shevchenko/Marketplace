namespace Marketplace.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderItem
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderItemId { get; set; }

    [Required] public int OrderId { get; set; }
    [ForeignKey("OrderId")] public Order Order { get; set; }

    [Required] public int ProductId { get; set; }
    [ForeignKey("ProductId")] public Product Product { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    [Required, Column(TypeName = "decimal(11,2)"), DataType(DataType.Currency)]
    public decimal Price { get; set; }
}