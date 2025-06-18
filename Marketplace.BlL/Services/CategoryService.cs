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

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        await _unitOfWork.CategoryRepository.CreateAsync(category);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.CategoryRepository.FindByIdAsync(id);
        if (category == null)
            throw new NotFoundException($"Category with id {id} not found.");

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.CategoryRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesSortedByNameAsync(bool ascending = true)
    {
        var categories = await _unitOfWork.CategoryRepository.FindCategoriesSortedByNameAsync(ascending);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task UpdateCategoryAsync(UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.CategoryRepository.FindByIdAsync(dto.CategoryId);
        if (category == null)
            throw new NotFoundException($"Category with id {dto.CategoryId} not found.");

        category.Name = dto.Name ?? category.Name;

        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _unitOfWork.CategoryRepository.FindByIdAsync(id);
        if (category == null)
            throw new NotFoundException($"Category with id {id} not found.");

        _unitOfWork.CategoryRepository.Delete(category);
        await _unitOfWork.SaveAsync();
    }
}