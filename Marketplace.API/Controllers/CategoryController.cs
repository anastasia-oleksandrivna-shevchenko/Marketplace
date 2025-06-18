using Marketplace.BLL.DTO.Category;
using Marketplace.BLL.Services.Interfaces;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SortByName([FromQuery] bool ascending, CancellationToken cancellationToken)
    {
        var categories = await _service.GetCategoriesSortedByNameAsync(ascending, cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var category = await _service.GetCategoryByIdAsync(id, cancellationToken);
        if (category == null)
            return NotFound(new { message = $"Category with id {id} not found." });
        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        var category = await _service.CreateCategoryAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = category.CategoryId}, category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        var existingCategory = await _service.GetCategoryByIdAsync(dto.CategoryId, cancellationToken);
        if (existingCategory == null)
            return NotFound(new { message = $"Category with id {dto.CategoryId} not found." });
        
        await _service.UpdateCategoryAsync(dto, cancellationToken); 
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var existingCategory = await _service.GetCategoryByIdAsync(id, cancellationToken);
        if (existingCategory == null)
            return NotFound(new { message = $"Category with id {id} not found." });
        
        await _service.DeleteCategoryAsync(id, cancellationToken); 
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _service.GetAllCategoriesAsync(cancellationToken);
        return Ok(categories);
    }
}