using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    public Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
    public Task<IEnumerable<Order>> GetOrdersByStoreIdAsync(int storeId);
    
}