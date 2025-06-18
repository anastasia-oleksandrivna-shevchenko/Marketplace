using Marketplace.BLL.DTO.User;
using Marketplace.DAL.Entities;

namespace Marketplace.BLL.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role);
    Task UpdateUserAsync(UpdateUserDto dto, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default);
}