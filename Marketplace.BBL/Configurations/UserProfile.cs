using AutoMapper;
using Marketplace.BBL.DTO.User;
using Marketplace.DAL.Entities;

namespace Marketplace.BBL.Configurations;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
    }
}