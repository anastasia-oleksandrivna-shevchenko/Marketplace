using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    public Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int storeId);
    public Task<IEnumerable<Product>> GetProductsByStoreIdAsync(int storeId);
    public Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    public Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    public Task<IEnumerable<Product>> GetProductsSortedByPriceAsync(bool ascending = true);
    public Task<IEnumerable<Product>> GetProductsSortedByRatingAsync(bool ascending = true);
    

}