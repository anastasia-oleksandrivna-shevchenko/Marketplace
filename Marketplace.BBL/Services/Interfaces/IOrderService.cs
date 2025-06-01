using Marketplace.BBL.DTO.Order;

namespace Marketplace.BBL.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
    Task<OrderDto> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(int customerId);
    Task<IEnumerable<OrderDto>> GetOrdersByStoreIdAsync(int storeId);
    Task UpdateOrderAsync(UpdateOrderStatusDto dto);
    Task DeleteOrderAsync(int id);
}
