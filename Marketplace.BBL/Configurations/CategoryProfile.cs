using AutoMapper;
using Marketplace.BBL.DTO.Category;
using Marketplace.DAL.Entities;

namespace Marketplace.BBL.Configurations;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
    }
}