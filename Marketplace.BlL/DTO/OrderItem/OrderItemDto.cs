namespace Marketplace.BLL.DTO.OrderItem;

public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } 
    public int Quantity { get; set; }
    public decimal Price { get; set; } 
    public decimal TotalPrice => Price * Quantity;
}