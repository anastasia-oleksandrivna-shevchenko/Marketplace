using AutoMapper;
using Marketplace.BLL.DTO.Store;
using Marketplace.BLL.DTO.Product;
using Marketplace.DAL.Entities;

namespace Marketplace.BLL.Configurations;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
    }
}