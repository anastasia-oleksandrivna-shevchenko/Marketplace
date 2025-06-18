using Marketplace.DAL.Entities;
using AutoMapper;
using Marketplace.BLL.DTO.Review;

namespace Marketplace.BLL.Configurations;

public class ReviewProfile:Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>();
        CreateMap<CreateReviewDto, Review>();
        CreateMap<UpdateReviewDto, Review>();
    }
}