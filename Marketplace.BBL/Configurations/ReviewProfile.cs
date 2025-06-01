using Marketplace.BBL.DTO.Review;
using Marketplace.DAL.Entities;
using AutoMapper;

namespace Marketplace.BBL.Configurations;

public class ReviewProfile:Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>();
        CreateMap<CreateReviewDto, Review>();
        CreateMap<UpdateReviewDto, Review>();
    }
}