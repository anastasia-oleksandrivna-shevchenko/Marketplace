using Marketplace.BLL.DTO.OrderItem;

namespace Marketplace.BLL.Services.Interfaces;

public interface IOrderItemService
{
    Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto dto, CancellationToken cancellationToken = default);
    Task UpdateOrderItemAsync(UpdateOrderItemDto dto, CancellationToken cancellationToken = default);
    Task DeleteOrderItemAsync(int id, CancellationToken cancellationToken = default);
    Task<OrderItemDto> GetOrderItemByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItemDto>> GetAllOrderItemsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItemDto>> GetItemsByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
}