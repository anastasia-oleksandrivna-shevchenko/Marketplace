using AutoMapper;
using Marketplace.BLL.DTO.Store;
using Marketplace.BLL.DTO.Product;
using Marketplace.DAL.Entities;

namespace Marketplace.BLL.Configurations;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
    }
}