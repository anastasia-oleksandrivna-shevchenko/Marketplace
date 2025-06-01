using AutoMapper;
using Marketplace.BBL.DTO.Store;
using Marketplace.BBL.DTO.User;
using Marketplace.DAL.Entities;

namespace Marketplace.BBL.Configurations;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreDto>();
        CreateMap<CreateStoreDto, Store>();
        CreateMap<UpdateStoreDto, Store>();
    }
}