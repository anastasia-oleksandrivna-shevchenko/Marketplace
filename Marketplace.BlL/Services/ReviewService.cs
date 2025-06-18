using AutoMapper;
using Marketplace.BLL.DTO.Review;
using Marketplace.BLL.Exceptions;
using Marketplace.BLL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.UnitOfWork;

namespace Marketplace.BLL.Services;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto, CancellationToken cancellationToken = default)
    {
        var review = _mapper.Map<Review>(dto);
        review.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.ReviewRepository.CreateAsync(review, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<ReviewDto>(review);
    }
    
    public async Task UpdateReviewAsync(UpdateReviewDto dto, CancellationToken cancellationToken = default)
    {
        var review = await _unitOfWork.ReviewRepository.FindByIdAsync(dto.ReviewId, cancellationToken);
        if (review == null)
            throw new NotFoundException($"Review with ID {dto.ReviewId} not found");


        review.Comment = dto.Comment ?? review.Comment;
        review.Rating = dto.Rating ?? review.Rating;

        await _unitOfWork.SaveAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync(CancellationToken cancellationToken = default)
    {
        var reviews = await _unitOfWork.ReviewRepository.FindReviewsWithUserAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
    
    public async Task<ReviewDto> GetReviewByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var review = await _unitOfWork.ReviewRepository.FindReviewByIdByWithUserAsync(id, cancellationToken);
        if (review == null)
            throw new NotFoundException($"Review with ID {id} not found");

        return _mapper.Map<ReviewDto>(review);
    }
    
    public async Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var reviews = await _unitOfWork.ReviewRepository.FindReviewsByProductIdAsync(productId, cancellationToken);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
    
    public async Task<IEnumerable<ReviewDto>> GetReviewsSortedByRatingAsync(bool ascending = false, CancellationToken cancellationToken = default)
    {
        var reviews = await _unitOfWork.ReviewRepository.FindReviewsSortedByRatingAsync(ascending, cancellationToken);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
    
    public async Task<IEnumerable<ReviewDto>> GetReviewsSortedByDateAsync(bool ascending = false, CancellationToken cancellationToken = default)
    {
        var reviews = await _unitOfWork.ReviewRepository.FindReviewsSortedByDateAsync(ascending, cancellationToken);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
    
    public async Task DeleteReviewAsync(int id, CancellationToken cancellationToken = default)
    {
        var review = await _unitOfWork.ReviewRepository.FindByIdAsync(id, cancellationToken);
        if (review == null)
            throw new NotFoundException($"Review with ID {id} not found");

        _unitOfWork.ReviewRepository.Delete(review, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}