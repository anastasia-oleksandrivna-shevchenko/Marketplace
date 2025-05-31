using Marketplace.Data;
using Marketplace.Entities;
using Marketplace.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories;

public class OrderItemRepository: GenericRepository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(MarketplaceDbContext context) : base(context) {}

    public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId)
    {
        return await _dbSet
            .Where(o => o.OrderId == orderId)
            .Include(oi => oi.Product)
            .ToListAsync();
    }
    
}