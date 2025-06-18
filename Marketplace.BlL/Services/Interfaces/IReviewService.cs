using Marketplace.BLL.DTO.Review;

namespace Marketplace.BLL.Services.Interfaces;

public interface IReviewService
{
    Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto);
    Task UpdateReviewAsync(UpdateReviewDto dto);
    Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
    Task<ReviewDto> GetReviewByIdAsync(int reviewId);
    Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId);
    Task<IEnumerable<ReviewDto>> GetReviewsSortedByRatingAsync(bool ascending = false);
    Task<IEnumerable<ReviewDto>> GetReviewsSortedByDateAsync(bool ascending = false);
    Task DeleteReviewAsync(int id);
    
}