using Marketplace.DAL.Data;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repositories;

public class UserRepository: GenericRepository<User>, IUserRepository
{
    public UserRepository(MarketplaceDbContext context) : base(context) {}
    
    private readonly UserManager<User> _userManager;

    public UserRepository(MarketplaceDbContext context, UserManager<User> userManager) : base(context)
    {
        _userManager = userManager;
    }

    public async Task<User?> FindUserByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }
    
    public async Task<User?> FindUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckUserExistsByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username) != null;
    }

    public async Task<bool> CheckUserExistsByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<IEnumerable<User>> FindUsersByRoleAsync(string role)
    {
        return await _userManager.GetUsersInRoleAsync(role);
    }
    
    public async Task<string?> FindRoleAsync(User user)
    {
        var role = await _userManager.GetRolesAsync(user);
        return role.FirstOrDefault();
    }
}