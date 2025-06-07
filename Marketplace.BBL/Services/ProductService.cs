using AutoMapper;
using Marketplace.BBL.DTO.Parameters;
using Marketplace.BBL.DTO.Product;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Helpers;
using Marketplace.DAL.Repositories.Interfaces;

namespace Marketplace.BBL.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Product> _sortHelper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ISortHelper<Product> sortHelper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }
    
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.ProductRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.FindByIdAsync(id);
        if (product == null) 
            throw new Exception("Product not found");
        return _mapper.Map<ProductDto>(product);
    }
    
    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.ProductRepository.CreateAsync(product);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<ProductDto>(product);
    }
    
    public async Task UpdateProductAsync(UpdateProductDto dto)
    {
        var product = await _unitOfWork.ProductRepository.FindByIdAsync(dto.ProductId);
        if (product == null) 
            throw new Exception("Product not found");

        product.Name = dto.Name ?? product.Name;
        product.Description = dto.Description ?? product.Description;
        product.Price = dto.Price ?? product.Price;
        product.Quantity = dto.Quantity ?? product.Quantity;
        product.CategoryId = dto.CategoryId ?? product.CategoryId;
        product.ImageUrl = dto.ImageUrl ?? product.ImageUrl;

        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.FindByIdAsync(id);
        if (product == null) 
            throw new Exception("Product not found");

        _unitOfWork.ProductRepository.Delete(product);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task<IEnumerable<ProductDto>> GetProductsByNameAsync(string name)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsByNameAsync(name);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsByCategoryIdAsync(categoryId);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
    public async Task<IEnumerable<ProductDto>> GetProductsByStoreIdAsync(int storeId)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsByStoreIdAsync(storeId);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
    public async Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsByPriceRangeAsync(minPrice, maxPrice);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsSortedByPriceAsync(bool ascending = true)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsSortedByPriceAsync(ascending);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsSortedByRatingAsync(bool ascending = true)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsSortedByRatingAsync(ascending);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
    public async Task<PagedList<ProductDto>> GetAllPaginatedAsync(ProductParameters parameters, CancellationToken cancellationToken = default)
    {
        var pagedProducts = await _unitOfWork.ProductRepository.GetAllPaginatedAsync(parameters, _sortHelper, cancellationToken);
        var dtoList = _mapper.Map<List<ProductDto>>(pagedProducts);
        return new PagedList<ProductDto>(dtoList, pagedProducts.TotalCount, pagedProducts.CurrentPage, pagedProducts.PageSize);
    }
}