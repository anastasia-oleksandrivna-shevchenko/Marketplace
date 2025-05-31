using Marketplace.Entities;

namespace Marketplace.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    public Task<User> GetUserByUsernameAsync(string username);
    public Task<User> GetUserByEmailAsync(string email);
    public Task<bool> CheckUserExistsByUsernameAsync(string username);
    public Task<bool> CheckUserExistsByEmailAsync(string email);
    public Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
}