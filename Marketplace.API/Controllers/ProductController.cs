using Marketplace.BLL.DTO.Product;
using Marketplace.BLL.Services.Interfaces;
using Marketplace.DAL.Entities.HelpModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { message = $"Product with id {id} not found." });
        return Ok(product);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAllProductsAsync();
        return Ok(products);
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody]CreateProductDto dto)
    {
        var products = await _service.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = products.ProductId }, products);
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdateProductDto dto)
    {
        var existing = await _service.GetProductByIdAsync(dto.ProductId);
        if (existing == null)
            return NotFound(new { message = $"Product with id {dto.ProductId} not found." });

        await _service.UpdateProductAsync(dto); 
        return NoContent();
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _service.GetProductByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Product with id {id} not found." });

        await _service.DeleteProductAsync(id); 
        return NoContent();
    }

    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ByCategory(int categoryId)
    {
        var products = await _service.GetProductsByCategoryIdAsync(categoryId);
        if(products.IsNullOrEmpty())
            return NotFound(new { message = $"Products with category id {categoryId} not found." });
        
        return Ok(products);
    }

    [HttpGet("store/{storeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ByStore(int storeId)
    {
        var products = await _service.GetProductsByStoreIdAsync(storeId);
        if(products.IsNullOrEmpty())
            return NotFound(new { message = $"Products with store id {storeId} not found." });
        return Ok(products);
    }

    [HttpGet("by-name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ByName([FromQuery] string name)
    {
        var products = await _service.GetProductsByNameAsync(name);
        if(products.IsNullOrEmpty())
            return NotFound(new { message = $"Products with name {name} not found." });
        
        return Ok(products);
    }

    [HttpGet("range-{min}/{max}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ByPriceRange([FromQuery] decimal min, [FromQuery] decimal max)
    {
        var products = await _service.GetProductsByPriceRangeAsync(min, max);
        if(products.IsNullOrEmpty())
            return NotFound(new { message = $"Products in range {min}-{max} not found." });
            
        return Ok(products);
    }

    [HttpGet("sort-by-price")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SortByPrice([FromQuery] bool ascending)
    {
        var products = await _service.GetProductsSortedByPriceAsync(ascending);
        return Ok(products);
    }

    [HttpGet("sort-by-rating")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SortByRating([FromQuery] bool ascending)
    {
        var products = await _service.GetProductsSortedByRatingAsync(ascending);
        return Ok(products);
    }
    
    [HttpGet("paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPaginated([FromQuery] ProductParameters parameters, CancellationToken cancellationToken)
    {
        if (parameters.PageNumber <= 0)
            parameters.PageNumber = 1;

        var products = await _service.GetAllPaginatedAsync(parameters, cancellationToken);
        return Ok(products);
    }

}