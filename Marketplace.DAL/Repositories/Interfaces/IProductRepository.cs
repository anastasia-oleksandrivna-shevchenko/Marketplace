using Marketplace.DAL.Entities;
using Marketplace.DAL.Entities.HelpModels;
using Marketplace.DAL.Helpers;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    public Task<List<Product>> FindByIdsAsync(List<int> ids, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Product>> FindProductsByCategoryIdAsync(int storeId, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Product>> FindProductsByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Product>> FindProductsByNameAsync(string name, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Product>> FindProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Product>> FindProductsSortedByPriceAsync(bool ascending = true, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Product>> FindProductsSortedByRatingAsync(bool ascending = true, CancellationToken cancellationToken = default);

    IQueryable<Product> ApplyFilters(IQueryable<Product> query, ProductParameters parameters);
    public Task<PagedList<Product>> GetAllPaginatedAsync(
        ProductParameters parameters,
        ISortHelper<Product> sortHelper,
        CancellationToken cancellationToken = default);


}