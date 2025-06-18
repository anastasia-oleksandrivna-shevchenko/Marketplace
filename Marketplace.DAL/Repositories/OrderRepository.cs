using Marketplace.DAL.Data;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repositories;

public class OrderRepository: GenericRepository<Order>, IOrderRepository
{
    public OrderRepository (MarketplaceDbContext context) : base(context) {}

    public async Task<IEnumerable<Order>> FindOrdersByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> FindOrdersByStoreIdAsync(int storeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.StoreId == storeId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync(cancellationToken);
    }
}