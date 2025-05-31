using Marketplace.BBL.DTO.OrderItem;

namespace Marketplace.BBL.DTO.Order;

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public int StoreId { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; }
}