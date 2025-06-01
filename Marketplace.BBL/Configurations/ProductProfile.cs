using AutoMapper;
using Marketplace.BBL.DTO.Product;
using Marketplace.BBL.DTO.Store;
using Marketplace.DAL.Entities;

namespace Marketplace.BBL.Configurations;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
    }
}