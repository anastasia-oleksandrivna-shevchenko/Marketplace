using AutoMapper;
using Marketplace.BLL.DTO.Category;
using Marketplace.DAL.Entities;

namespace Marketplace.BLL.Configurations;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
    }
}