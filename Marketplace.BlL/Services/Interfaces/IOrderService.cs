using Marketplace.BLL.DTO.Order;

namespace Marketplace.BLL.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, CancellationToken cancellationToken = default);
    Task<OrderDto> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderDto>> GetOrdersByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
    Task UpdateOrderAsync(UpdateOrderStatusDto dto, CancellationToken cancellationToken = default);
    Task DeleteOrderAsync(int id, CancellationToken cancellationToken = default);
}
