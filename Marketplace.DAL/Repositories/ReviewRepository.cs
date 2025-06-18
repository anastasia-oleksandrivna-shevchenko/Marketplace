using Marketplace.DAL.Data;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repositories;

public class ReviewRepository: GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(MarketplaceDbContext context) : base(context) {}

    public async Task<IEnumerable<Review>> FindReviewsWithUserAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.User)
            .ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<Review>> FindReviewsByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.ProductId == productId)
            .Include(r => r.User)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Review?> FindReviewByIdByWithUserAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.ReviewId == id)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.ReviewId == id, cancellationToken);
    }
    
    public async Task<IEnumerable<Review>> FindReviewsSortedByRatingAsync(bool ascending = false, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.User)
            .OrderBy(r => ascending? r.Rating : -r.Rating)
            .ToListAsync(cancellationToken);
            
    }

    public async Task<IEnumerable<Review>> FindReviewsSortedByDateAsync(bool ascending = false, CancellationToken cancellationToken = default)
    {
        return await (ascending
                ? _dbSet.OrderBy(r => r.CreatedAt)
                : _dbSet.OrderByDescending(r => r.CreatedAt))
            .Include(r => r.User)
            .ToListAsync(cancellationToken);
    }
}