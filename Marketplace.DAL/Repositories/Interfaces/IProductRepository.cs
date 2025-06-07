using Marketplace.BBL.DTO.Parameters;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Helpers;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    public Task<IEnumerable<Product>> FindProductsByCategoryIdAsync(int storeId);
    public Task<IEnumerable<Product>> FindProductsByStoreIdAsync(int storeId);
    public Task<IEnumerable<Product>> FindProductsByNameAsync(string name);
    public Task<IEnumerable<Product>> FindProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    public Task<IEnumerable<Product>> FindProductsSortedByPriceAsync(bool ascending = true);
    public Task<IEnumerable<Product>> FindProductsSortedByRatingAsync(bool ascending = true);

    public Task<PagedList<Product>> GetAllPaginatedAsync(
        ProductParameters parameters,
        ISortHelper<Product> sortHelper,
        CancellationToken cancellationToken = default);


}