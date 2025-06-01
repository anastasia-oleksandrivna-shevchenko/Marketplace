using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IStoreRepository : IGenericRepository<Store>
{
    public Task<IEnumerable<Store>> FindStoresByUserIdAsync(int userId);
    public Task<IEnumerable<Store>> FindStoresSortedByOrdersCountAsync(bool ascending = false);
    public Task<IEnumerable<Store>> FindStoresSortedByRatingAsync(bool ascending = false);
    public Task<IEnumerable<Store>> FindStoresByNameAsync(string name);
}