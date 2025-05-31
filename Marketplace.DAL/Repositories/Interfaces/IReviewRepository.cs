using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IReviewRepository : IGenericRepository<Review>
{
    public Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
    public Task<IEnumerable<Review>> GetReviewsSortedByRatingAsync(bool ascending = false);
    public Task<IEnumerable<Review>> GetReviewsSortedByDateAsync(bool ascending = false);
    
}