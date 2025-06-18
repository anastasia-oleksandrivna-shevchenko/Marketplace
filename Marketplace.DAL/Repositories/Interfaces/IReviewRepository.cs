using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IReviewRepository : IGenericRepository<Review>
{
    public Task<IEnumerable<Review>> FindReviewsByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Review>> FindReviewsSortedByRatingAsync(bool ascending = false, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Review>> FindReviewsSortedByDateAsync(bool ascending = false, CancellationToken cancellationToken = default);
    
}