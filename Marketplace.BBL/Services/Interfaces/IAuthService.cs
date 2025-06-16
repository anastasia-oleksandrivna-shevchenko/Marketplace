using Marketplace.BBL.DTO.User;

namespace Marketplace.BBL.Services.Interfaces;

public interface IAuthService
{
    Task RegisterUserAsync(CreateUserDto model);
}
