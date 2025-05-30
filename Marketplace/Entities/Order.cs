namespace Marketplace.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }

    [Required] public int CustomerId { get; set; }
    [ForeignKey("CustomerId")] public User Customer { get; set; }

    [Required] public int StoreId { get; set; }
    [ForeignKey("StoreId")] public Store Store { get; set; }

    [Required] public DateTime OrderDate { get; set; }

    [Required, Column(TypeName = "decimal(11,2)"), DataType(DataType.Currency)]
    public decimal TotalPrice { get; set; }

    [Required, MaxLength(20), RegularExpression("Pending|Shipped|Completed")]
    public string Status { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; }
}
    