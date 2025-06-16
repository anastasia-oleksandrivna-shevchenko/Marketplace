using Marketplace.BBL.DTO.User;

namespace Marketplace.BBL.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string? Error)> RegisterUserAsync(CreateUserDto model);
}
