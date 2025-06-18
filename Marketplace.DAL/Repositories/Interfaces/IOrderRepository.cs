using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    public Task<IEnumerable<Order>> FindOrdersByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Order>> FindOrdersByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
    
}