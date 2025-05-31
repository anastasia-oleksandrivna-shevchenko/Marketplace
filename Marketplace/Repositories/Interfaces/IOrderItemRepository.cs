using Marketplace.Entities;

namespace Marketplace.Repositories.Interfaces;

public interface IOrderItemRepository : IGenericRepository<OrderItem>
{
    Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId);
    
}