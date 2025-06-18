using AutoMapper;
using Marketplace.BBL.DTO.Review;
using Marketplace.BBL.Exceptions;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.UnitOfWork;

namespace Marketplace.BBL.Services;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto)
    {
        var review = _mapper.Map<Review>(dto);
        review.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.ReviewRepository.CreateAsync(review);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ReviewDto>(review);
    }
    
    public async Task UpdateReviewAsync(UpdateReviewDto dto)
    {
        var review = await _unitOfWork.ReviewRepository.FindByIdAsync(dto.ReviewId);
        if (review == null)
            throw new NotFoundException($"Review with ID {dto.ReviewId} not found");


        review.Comment = dto.Comment ?? review.Comment;
        review.Rating = dto.Rating ?? review.Rating;

        await _unitOfWork.SaveAsync();
    }
    
    public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
    {
        var reviews = await _unitOfWork.ReviewRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
    
    public async Task<ReviewDto> GetReviewByIdAsync(int id)
    {
        var review = await _unitOfWork.ReviewRepository.FindByIdAsync(id);
        if (review == null)
            throw new NotFoundException($"Review with ID {id} not found");

        return _mapper.Map<ReviewDto>(review);
    }
    
    public async Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId)
    {
        var reviews = await _unitOfWork.ReviewRepository.FindReviewsByProductIdAsync(productId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
    
    public async Task<IEnumerable<ReviewDto>> GetReviewsSortedByRatingAsync(bool ascending = false)
    {
        var reviews = await _unitOfWork.ReviewRepository.FindReviewsSortedByRatingAsync(ascending);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
    
    public async Task<IEnumerable<ReviewDto>> GetReviewsSortedByDateAsync(bool ascending = false)
    {
        var reviews = await _unitOfWork.ReviewRepository.FindReviewsSortedByDateAsync(ascending);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
    
    public async Task DeleteReviewAsync(int id)
    {
        var review = await _unitOfWork.ReviewRepository.FindByIdAsync(id);
        if (review == null)
            throw new NotFoundException($"Review with ID {id} not found");

        _unitOfWork.ReviewRepository.Delete(review);
        await _unitOfWork.SaveAsync();
    }
}