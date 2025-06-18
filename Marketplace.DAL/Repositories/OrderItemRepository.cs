using Marketplace.DAL.Data;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repositories;

public class OrderItemRepository: GenericRepository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(MarketplaceDbContext context) : base(context) {}

    public async Task<IEnumerable<OrderItem>> FindItemsByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.OrderId == orderId)
            .Include(oi => oi.Product)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<OrderItem>> FindOrderItemsWithProductsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(oi => oi.Product)
            .ToListAsync(cancellationToken);
    }
    public async Task<OrderItem?> FindOrderItemWithProductsByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.OrderItemId == id)
            .Include(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderItemId == id, cancellationToken);
    }
    
}