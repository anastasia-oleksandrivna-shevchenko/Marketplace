using Marketplace.BBL.DTO.Product;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var products = await _service.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = products.ProductId }, products);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateProductDto dto)
    {
        await _service.UpdateProductAsync(dto); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteProductAsync(id); 
        return NoContent();
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> ByCategory(int categoryId)
    {
        var products = await _service.GetProductsByCategoryIdAsync(categoryId);
        return Ok(products);
    }

    [HttpGet("store/{storeId}")]
    public async Task<IActionResult> ByStore(int storeId)
    {
        var products = await _service.GetProductsByStoreIdAsync(storeId);
        return Ok(products);
    }

    [HttpGet("by-name/{name}")]
    public async Task<IActionResult> ByName([FromQuery] string name)
    {
        var products = await _service.GetProductsByNameAsync(name);
        return Ok(products);
    }

    [HttpGet("range-{min}/{max}")]
    public async Task<IActionResult> ByPriceRange([FromQuery] decimal min, [FromQuery] decimal max)
    {
        var products = await _service.GetProductsByPriceRangeAsync(min, max);
        return Ok(products);
    }

    [HttpGet("sort-by-price")]
    public async Task<IActionResult> SortByPrice([FromQuery] bool ascending)
    {
        var products = await _service.GetProductsSortedByPriceAsync(ascending);
        return Ok(products);
    }

    [HttpGet("sort-by-rating")]
    public async Task<IActionResult> SortByRating([FromQuery] bool ascending)
    {
        var products = await _service.GetProductsSortedByRatingAsync(ascending);
        return Ok(products);
    }
}