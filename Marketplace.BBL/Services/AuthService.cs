using Marketplace.BBL.DTO.User;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.BBL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;

    public AuthService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<(bool Success, string? Error)> RegisterUserAsync(CreateUserDto model)
    {
        if (await _userManager.FindByNameAsync(model.Username) != null)
            return (false, "User with such name exist!");

        if (await _userManager.FindByEmailAsync(model.Email) != null)
            return (false, "User with such email exist!");

        if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.Phone))
            return (false, "User with such phone number exist!");

        var user = new User
        {
            UserName = model.Username,
            Email = model.Email,
            PhoneNumber = model.Phone,
            FirstName = model.FirstName,
            LastName = model.LastName,
            MiddleName = model.MiddleName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return (false, string.Join("; ", result.Errors.Select(e => e.Description)));

        if (!string.IsNullOrEmpty(model.Role))
        {
            var allowedRoles = new[] { "Admin", "Buyer", "Seller" };

            if (!allowedRoles.Contains(model.Role))
                return (false, "Invalid role.");

            await _userManager.AddToRoleAsync(user, model.Role);
        }

        return (true, null);
    }
}
