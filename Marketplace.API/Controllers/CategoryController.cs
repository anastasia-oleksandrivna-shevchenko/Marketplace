using Marketplace.BBL.DTO.Category;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers;

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

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var category = await _service.CreateCategoryAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = category.CategoryId}, category);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var existingCategory = await _service.GetCategoryByIdAsync(dto.CategoryId);
        if (existingCategory == null)
            return NotFound(new { message = $"Category with id {dto.CategoryId} not found." });
        
        await _service.UpdateCategoryAsync(dto); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingCategory = await _service.GetCategoryByIdAsync(id);
        if (existingCategory == null)
            return NotFound(new { message = $"Category with id {id} not found." });
        
        await _service.DeleteCategoryAsync(id); 
        return NoContent();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllCategoriesAsync();
        return Ok(categories);
    }
}