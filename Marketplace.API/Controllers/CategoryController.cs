using Marketplace.BBL.DTO.Category;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet("sorted-by-name")]
    public async Task<IActionResult> SortByName([FromQuery] bool ascending)
    {
        var categories = await _service.GetCategoriesSortedByNameAsync(ascending);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound(new { message = $"Category with id {id} not found." });
        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        var category = await _service.CreateCategoryAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = category.CategoryId}, category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryDto dto)
    {
        var existingCategory = await _service.GetCategoryByIdAsync(dto.CategoryId);
        if (existingCategory == null)
            return NotFound(new { message = $"Category with id {dto.CategoryId} not found." });
        
        await _service.UpdateCategoryAsync(dto); 
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingCategory = await _service.GetCategoryByIdAsync(id);
        if (existingCategory == null)
            return NotFound(new { message = $"Category with id {id} not found." });
        
        await _service.DeleteCategoryAsync(id); 
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllCategoriesAsync();
        return Ok(categories);
    }
}