using Marketplace.Data;
using Marketplace.Entities;
using Marketplace.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories;

public class UserRepository: GenericRepository<User>, IUserRepository
{
    public UserRepository(MarketplaceDbContext context) : base(context) {}

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Username == username);
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> CheckUserExistsByUsernameAsync(string username)
    {
        return await _dbSet
            .AnyAsync(u => u.Username == username);
    }

    public async Task<bool> CheckUserExistsByEmailAsync(string email)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
    {
        return await _dbSet
            .Where(u => u.Role == role)
            .ToListAsync();
    }
}