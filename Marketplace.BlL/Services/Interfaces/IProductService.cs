using Marketplace.BLL.DTO.Product;
using Marketplace.DAL.Entities.HelpModels;
using Marketplace.DAL.Helpers;

namespace Marketplace.BLL.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDto> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task UpdateProductAsync(UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetProductsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetProductsByStoreIdAsync(int storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetProductsSortedByPriceAsync(bool ascending = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductDto>> GetProductsSortedByRatingAsync(bool ascending = true, CancellationToken cancellationToken = default);
    Task<PagedList<ProductDto>> GetAllPaginatedAsync(ProductParameters parameters, CancellationToken cancellationToken = default);
}