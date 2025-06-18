using Marketplace.BBL.DTO.Auth;
using Marketplace.BBL.DTO.User;
using Marketplace.DAL.Entities;

namespace Marketplace.BBL.Services.Interfaces;

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