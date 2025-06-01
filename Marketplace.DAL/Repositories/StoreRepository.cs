using Marketplace.DAL.Data;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repositories;

public class StoreRepository: GenericRepository<Store>, IStoreRepository
{
    public StoreRepository(MarketplaceDbContext context) : base(context) {}

    public async Task<IEnumerable<Store>> FindStoresByUserIdAsync(int userId)
    {
        return await _dbSet
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Store>> FindStoresSortedByOrdersCountAsync(bool ascending = false)
    {
        return await _dbSet
            .Include(s => s.Orders)
            .OrderBy(s => ascending? s.Orders.Count : -s.Orders.Count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Store>> FindStoresSortedByRatingAsync(bool ascending = false)
    {
        return await _dbSet
            .OrderBy(s => ascending? s.Rating : -s.Rating)
            .ToListAsync();
    }

    public async Task<IEnumerable<Store>> FindStoresByNameAsync(string name)
    {
        return await _dbSet
            .Where(s => s.StoreName.Contains(name))
            .ToListAsync();
            
    }
    
}