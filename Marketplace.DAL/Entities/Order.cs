namespace Marketplace.DAL.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public User Customer { get; set; }
    public int StoreId { get; set; }
    public Store Store { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; }
}
    