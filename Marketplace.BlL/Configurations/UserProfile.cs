using AutoMapper;
using Marketplace.BLL.DTO.Auth;
using Marketplace.BLL.DTO.User;
using Marketplace.DAL.Entities;

namespace Marketplace.BLL.Configurations;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));
        CreateMap<RegisterRequestDto, User>();
        CreateMap<UpdateUserDto, User>();
    }
}