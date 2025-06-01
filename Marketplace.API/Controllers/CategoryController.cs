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
    public async Task<IActionResult> Sort([FromQuery] bool ascending)
    {
        var categories = await _service.GetCategoriesSortedByNameAsync(ascending);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var category = await _service.CreateCategoryAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = category.CategoryId}, category);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateCategoryDto dto)
    {
        await _service.UpdateCategoryAsync(dto); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
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