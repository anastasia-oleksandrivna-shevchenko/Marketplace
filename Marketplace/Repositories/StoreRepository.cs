using Marketplace.Data;
using Marketplace.Entities;
using Marketplace.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories;

public class StoreRepository: GenericRepository<Store>, IStoreRepository
{
    public StoreRepository(MarketplaceDbContext context) : base(context) {}

    public async Task<IEnumerable<Store>> GetStoresByUserIdAsync(int userId)
    {
        return await _dbSet
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Store>> GetStoresSortedByOrdersCountAsync(bool ascending = false)
    {
        return await _dbSet
            .Include(s => s.Orders)
            .OrderBy(s => ascending? s.Orders.Count : -s.Orders.Count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Store>> GetStoresSortedByRatingAsync(bool ascending = false)
    {
        return await _dbSet
            .OrderBy(s => ascending? s.Rating : -s.Rating)
            .ToListAsync();
    }
    
}