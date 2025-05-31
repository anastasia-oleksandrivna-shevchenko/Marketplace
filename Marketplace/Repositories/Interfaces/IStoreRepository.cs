using Marketplace.Entities;

namespace Marketplace.Repositories.Interfaces;

public interface IStoreRepository : IGenericRepository<Store>
{
    public Task<IEnumerable<Store>> GetStoresByUserIdAsync(int userId);
    public Task<IEnumerable<Store>> GetStoresSortedByOrdersCountAsync(bool ascending = false);
    public Task<IEnumerable<Store>> GetStoresSortedByRatingAsync(bool ascending = false);
}