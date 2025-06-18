using Marketplace.BLL.DTO.Review;

namespace Marketplace.BLL.Services.Interfaces;

public interface IReviewService
{
    Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto, CancellationToken cancellationToken = default);
    Task UpdateReviewAsync(UpdateReviewDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReviewDto>> GetAllReviewsAsync(CancellationToken cancellationToken = default);
    Task<ReviewDto> GetReviewByIdAsync(int reviewId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReviewDto>> GetReviewsSortedByRatingAsync(bool ascending = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReviewDto>> GetReviewsSortedByDateAsync(bool ascending = false, CancellationToken cancellationToken = default);
    Task DeleteReviewAsync(int id, CancellationToken cancellationToken = default);
    
}