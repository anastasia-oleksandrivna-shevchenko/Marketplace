using Marketplace.BBL.DTO.User;
using Marketplace.BBL.Exceptions;
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

    public async Task RegisterUserAsync(CreateUserDto model)
    {
        if (await _userManager.FindByNameAsync(model.Username) != null)
            throw new ConflictException("User with such name already exists.");
        if (await _userManager.FindByEmailAsync(model.Email) != null)
            throw new ConflictException("User with such email already exists.");
        if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.Phone))
            throw new ConflictException("User with such phone number already exists.");

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
            throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.Description)));

        if (!string.IsNullOrEmpty(model.Role))
        {
            var allowedRoles = new[] { "Admin", "Buyer", "Seller" };

            if (!allowedRoles.Contains(model.Role))
                throw new ArgumentException("Invalid role specified.");

            await _userManager.AddToRoleAsync(user, model.Role);
        }
    }
}
