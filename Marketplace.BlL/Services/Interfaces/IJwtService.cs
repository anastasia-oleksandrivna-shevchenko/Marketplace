using Marketplace.BLL.DTO.Auth;
using Marketplace.BLL.DTO.User;
using Marketplace.DAL.Entities;

namespace Marketplace.BLL.Services.Interfaces;

public interface IJwtService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
    Task RegisterUserAsync(CreateUserDto createUserDto);
    Task<RefreshTokenResponseDto> RefreshTokenAsync(string ipAddress);
    Task<bool> ConfirmEmailAsync(string userId, string token);
    Task<bool> ForgotPasswordAsync(string email);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    Task LogoutAsync();
}