using Marketplace.DAL.Data;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repositories;

public class UserRepository: GenericRepository<User>, IUserRepository
{
    public UserRepository(MarketplaceDbContext context) : base(context) {}
    
    public UserManager<User> _userManager { get; }

    public async Task<User?> FindUserByUsernameAsync(string username)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.UserName == username);
    }
    
    public async Task<User?> FindUserByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> CheckUserExistsByUsernameAsync(string username)
    {
        return await _dbSet
            .AnyAsync(u => u.UserName == username);
    }

    public async Task<bool> CheckUserExistsByEmailAsync(string email)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> FindUsersByRoleAsync(string role)
    {
        return await _userManager.GetUsersInRoleAsync(role);
    }
}