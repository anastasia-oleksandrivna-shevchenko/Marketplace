using Marketplace.Entities;

namespace Marketplace.Repositories.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    public Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
    public Task<IEnumerable<Order>> GetOrdersByStoreIdAsync(int storeId);
    
}