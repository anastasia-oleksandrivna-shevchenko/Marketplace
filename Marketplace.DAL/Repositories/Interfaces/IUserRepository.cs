using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    public Task<User> FindUserByUsernameAsync(string username);
    public Task<User> FindUserByEmailAsync(string email);
    public Task<bool> CheckUserExistsByUsernameAsync(string username);
    public Task<bool> CheckUserExistsByEmailAsync(string email);
    public Task<IEnumerable<User>> FindUsersByRoleAsync(string role);
}