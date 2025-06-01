using Marketplace.BBL.DTO.Product;

namespace Marketplace.BBL.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task UpdateProductAsync(UpdateProductDto dto);
    Task DeleteProductAsync(int id);
    Task<IEnumerable<ProductDto>> GetProductsByNameAsync(string name);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(int categoryId);
    Task<IEnumerable<ProductDto>> GetProductsByStoreIdAsync(int storeId);
    Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<IEnumerable<ProductDto>> GetProductsSortedByPriceAsync(bool ascending = true);
    Task<IEnumerable<ProductDto>> GetProductsSortedByRatingAsync(bool ascending = true);
}