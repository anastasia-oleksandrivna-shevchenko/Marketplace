using Marketplace.BBL.DTO.User;
using Marketplace.DAL.Entities;

namespace Marketplace.BBL.Services.Interfaces;

public interface IJwtService
{
    //Task<LoginDto> Login(LoginDto dto);
    //Task<bool> RegisterUserAsync(CreateUserDto dto);
    Task<string> GenerateToken(User user);
    //Task LogoutAsync();
}