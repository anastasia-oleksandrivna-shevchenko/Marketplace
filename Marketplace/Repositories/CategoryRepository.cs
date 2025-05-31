using Marketplace.Data;
using Marketplace.Entities;
using Marketplace.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories;

public class CategoryRepository: GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(MarketplaceDbContext context) : base(context){}

    public async Task<IEnumerable<Category>> GetCategoriesSortedByNameAsync(bool ascending = true)
    {
        return await (ascending 
                ? _dbSet.OrderBy(c => c.Name) 
                : _dbSet.OrderByDescending(c => c.Name))
            .ToListAsync();
    }
}