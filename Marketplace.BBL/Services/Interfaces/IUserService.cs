﻿using Marketplace.BBL.DTO.User;

namespace Marketplace.BBL.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> RegisterUserAsync(CreateUserDto dto);
    Task<UserDto> GetUserByIdAsync(int userId);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role);
    Task UpdateUserAsync(UpdateUserDto dto);
    Task ChangePasswordAsync(ChangePasswordDto dto);
    Task ChangeEmailAsync(ChangeEmailDto dto);
    Task ChangeUsernameAsync(ChangeUsernameDto dto);
    Task DeleteUserAsync(int userId);
}