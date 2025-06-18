using AutoMapper;
using Marketplace.BLL.DTO.User;
using Marketplace.BLL.DTO.Store;
using Marketplace.DAL.Entities;

namespace Marketplace.BLL.Configurations;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreDto>();
        CreateMap<CreateStoreDto, Store>();
        CreateMap<UpdateStoreDto, Store>();
    }
}