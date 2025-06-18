using Marketplace.DAL.Data;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repositories;

public class CategoryRepository: GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(MarketplaceDbContext context) : base(context){}

    public async Task<IEnumerable<Category>> FindCategoriesSortedByNameAsync(bool ascending = true, CancellationToken cancellationToken = default)
    {
        return await (ascending 
                ? _dbSet.OrderBy(c => c.Name) 
                : _dbSet.OrderByDescending(c => c.Name))
            .ToListAsync(cancellationToken);
    }
}