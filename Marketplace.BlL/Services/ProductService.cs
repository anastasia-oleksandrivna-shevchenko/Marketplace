using AutoMapper;
using Marketplace.BLL.DTO.Product;
using Marketplace.BLL.Exceptions;
using Marketplace.BLL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Entities.HelpModels;
using Marketplace.DAL.Helpers;
using Marketplace.DAL.UnitOfWork;

namespace Marketplace.BLL.Services;

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
    
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.ProductRepository.FindAllWithStoreAndCategory(cancellationToken);
        return _mapper.Map<List<ProductDto>>(products);
    }
    
    public async Task<ProductDto> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.ProductRepository.FindByIdWithStoreAndCategoryAsync(id, cancellationToken);
        if (product == null)
            throw new NotFoundException($"Product with ID {id} not found");
        
        return _mapper.Map<ProductDto>(product);
    }
    
    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.ProductRepository.CreateAsync(product, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<ProductDto>(product);
    }
    
    public async Task UpdateProductAsync(UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.ProductRepository.FindByIdAsync(dto.ProductId, cancellationToken);
        if (product == null)
            throw new NotFoundException($"Product with ID {dto.ProductId} not found");

        product.Name = dto.Name ?? product.Name;
        product.Description = dto.Description ?? product.Description;
        product.Price = dto.Price ?? product.Price;
        product.Quantity = dto.Quantity ?? product.Quantity;
        product.CategoryId = dto.CategoryId ?? product.CategoryId;
        product.ImageUrl = dto.ImageUrl ?? product.ImageUrl;

        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.ProductRepository.FindByIdAsync(id, cancellationToken);
        if (product == null)
            throw new NotFoundException($"Product with ID {id} not found");

        _unitOfWork.ProductRepository.Delete(product, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<ProductDto>> GetProductsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsByNameAsync(name, cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsByCategoryIdAsync(categoryId, cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
    public async Task<IEnumerable<ProductDto>> GetProductsByStoreIdAsync(int storeId, CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsByStoreIdAsync(storeId, cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
    public async Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsByPriceRangeAsync(minPrice, maxPrice, cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsSortedByPriceAsync(bool ascending = true, CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsSortedByPriceAsync(ascending, cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsSortedByRatingAsync(bool ascending = true, CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.ProductRepository.FindProductsSortedByRatingAsync(ascending, cancellationToken);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
    public async Task<PagedList<ProductDto>> GetAllPaginatedAsync(ProductParameters parameters, CancellationToken cancellationToken = default)
    {
        var pagedProducts = await _unitOfWork.ProductRepository.GetAllPaginatedAsync(parameters, _sortHelper, cancellationToken);
        var dtoList = _mapper.Map<List<ProductDto>>(pagedProducts);
        return new PagedList<ProductDto>(dtoList, pagedProducts.TotalCount, pagedProducts.CurrentPage, pagedProducts.PageSize);
    }
}