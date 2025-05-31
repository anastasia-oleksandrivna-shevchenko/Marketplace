using Marketplace.BBL.DTO.OrderItem;

namespace Marketplace.BBL.DTO.Order;

public class OrderDto
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerEmail { get; set; }
    public int StoreId { get; set; }
    public string StoreName { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }

    public List<OrderItemDto> OrderItems { get; set; }
}