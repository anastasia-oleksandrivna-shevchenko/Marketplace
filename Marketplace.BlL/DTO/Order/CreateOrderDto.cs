using Marketplace.BLL.DTO.OrderItem;

namespace Marketplace.BLL.DTO.Order;

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public int StoreId { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; }
}