using AutoMapper;
using Marketplace.BLL.DTO.Category;
using Marketplace.BLL.Exceptions;
using Marketplace.BLL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.UnitOfWork;

namespace Marketplace.BLL.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = _mapper.Map<Category>(dto);
        await _unitOfWork.CategoryRepository.CreateAsync(category, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.CategoryRepository.FindByIdAsync(id, cancellationToken);
        if (category == null)
            throw new NotFoundException($"Category with id {id} not found.");

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.CategoryRepository.FindAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesSortedByNameAsync(bool ascending = true, CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.CategoryRepository.FindCategoriesSortedByNameAsync(ascending, cancellationToken);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task UpdateCategoryAsync(UpdateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.CategoryRepository.FindByIdAsync(dto.CategoryId, cancellationToken);
        if (category == null)
            throw new NotFoundException($"Category with id {dto.CategoryId} not found.");

        category.Name = dto.Name ?? category.Name;

        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.CategoryRepository.FindByIdAsync(id, cancellationToken);
        if (category == null)
            throw new NotFoundException($"Category with id {id} not found.");

        _unitOfWork.CategoryRepository.Delete(category, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}