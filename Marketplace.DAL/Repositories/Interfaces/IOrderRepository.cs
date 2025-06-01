using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    public Task<IEnumerable<Order>> FindOrdersByCustomerIdAsync(int customerId);
    public Task<IEnumerable<Order>> FindOrdersByStoreIdAsync(int storeId);
    
}