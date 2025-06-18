using Marketplace.BLL.DTO.OrderItem;

namespace Marketplace.BLL.Services.Interfaces;

public interface IOrderItemService
{
    Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto dto);
    Task UpdateOrderItemAsync(UpdateOrderItemDto dto);
    Task DeleteOrderItemAsync(int id);
    Task<OrderItemDto> GetOrderItemByIdAsync(int id);
    Task<IEnumerable<OrderItemDto>> GetAllOrderItemsAsync();
    Task<IEnumerable<OrderItemDto>> GetItemsByOrderIdAsync(int orderId);
}