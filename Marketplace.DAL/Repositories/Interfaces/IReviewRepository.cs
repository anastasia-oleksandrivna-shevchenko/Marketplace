using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IReviewRepository : IGenericRepository<Review>
{
    public Task<IEnumerable<Review>> FindReviewsByProductIdAsync(int productId);
    public Task<IEnumerable<Review>> FindReviewsSortedByRatingAsync(bool ascending = false);
    public Task<IEnumerable<Review>> FindReviewsSortedByDateAsync(bool ascending = false);
    
}