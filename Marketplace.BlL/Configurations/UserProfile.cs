using AutoMapper;
using Marketplace.BLL.DTO.User;
using Marketplace.DAL.Entities;

namespace Marketplace.BLL.Configurations;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
    }
}