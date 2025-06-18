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
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _service.GetProductByIdAsync(id, cancellationToken);
        if (product == null)
            return NotFound(new { message = $"Product with id {id} not found." });
        return Ok(product);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await _service.GetAllProductsAsync(cancellationToken);
        return Ok(products);
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody]CreateProductDto dto, CancellationToken cancellationToken)
    {
        var products = await _service.CreateProductAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = products.ProductId }, products);
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdateProductDto dto, CancellationToken cancellationToken)
    {
        var existing = await _service.GetProductByIdAsync(dto.ProductId, cancellationToken);
        if (existing == null)
            return NotFound(new { message = $"Product with id {dto.ProductId} not found." });

        await _service.UpdateProductAsync(dto, cancellationToken); 
        return NoContent();
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var existing = await _service.GetProductByIdAsync(id, cancellationToken);
        if (existing == null)
            return NotFound(new { message = $"Product with id {id} not found." });

        await _service.DeleteProductAsync(id, cancellationToken); 
        return NoContent();
    }

    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ByCategory(int categoryId, CancellationToken cancellationToken)
    {
        var products = await _service.GetProductsByCategoryIdAsync(categoryId, cancellationToken);
        if(products.IsNullOrEmpty())
            return NotFound(new { message = $"Products with category id {categoryId} not found." });
        
        return Ok(products);
    }

    [HttpGet("store/{storeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ByStore(int storeId, CancellationToken cancellationToken)
    {
        var products = await _service.GetProductsByStoreIdAsync(storeId, cancellationToken);
        if(products.IsNullOrEmpty())
            return NotFound(new { message = $"Products with store id {storeId} not found." });
        return Ok(products);
    }

    [HttpGet("by-name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ByName([FromQuery] string name, CancellationToken cancellationToken)
    {
        var products = await _service.GetProductsByNameAsync(name, cancellationToken);
        if(products.IsNullOrEmpty())
            return NotFound(new { message = $"Products with name {name} not found." });
        
        return Ok(products);
    }

    [HttpGet("range-{min}/{max}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ByPriceRange([FromQuery] decimal min, [FromQuery] decimal max, CancellationToken cancellationToken)
    {
        var products = await _service.GetProductsByPriceRangeAsync(min, max,cancellationToken);
        if(products.IsNullOrEmpty())
            return NotFound(new { message = $"Products in range {min}-{max} not found." });
            
        return Ok(products);
    }

    [HttpGet("sort-by-price")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SortByPrice([FromQuery] bool ascending, CancellationToken cancellationToken)
    {
        var products = await _service.GetProductsSortedByPriceAsync(ascending, cancellationToken);
        return Ok(products);
    }

    [HttpGet("sort-by-rating")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SortByRating([FromQuery] bool ascending, CancellationToken cancellationToken)
    {
        var products = await _service.GetProductsSortedByRatingAsync(ascending, cancellationToken);
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