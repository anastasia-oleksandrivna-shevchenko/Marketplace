using Marketplace.DAL.Data;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repositories;

public class OrderRepository: GenericRepository<Order>, IOrderRepository
{
    public OrderRepository (MarketplaceDbContext context) : base(context) {}

    public async Task<IEnumerable<Order>> FindOrdersWithUserAndStoreAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.Store)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync(cancellationToken);
    }
    public async Task<Order?> FindOrdersByIdWithUserAndStoreAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.OrderId == id)
            .Include(o => o.Customer)
            .Include(o => o.Store)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id, cancellationToken);
    }
    
    public async Task<IEnumerable<Order>> FindOrdersByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Customer)
            .Include(o => o.Store)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> FindOrdersByStoreIdAsync(int storeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.StoreId == storeId)
            .Include(o => o.Customer)
            .Include(o => o.Store)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync(cancellationToken);
    }
}