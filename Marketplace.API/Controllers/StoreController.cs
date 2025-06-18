using Marketplace.BLL.DTO.Store;
using Marketplace.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreController : ControllerBase
{
    private readonly IStoreService _service;

    public StoreController(IStoreService service)
    {
        _service = service;
    } 

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var store = await _service.GetStoreByIdAsync(id, cancellationToken);
        if (store == null)
            return NotFound(new { message = $"Store with id {id} not found." });
        return Ok(store);
    }

    [HttpGet("by-name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
    {
        var stores = await _service.GetStoresByNameAsync(name, cancellationToken);
        if (stores.IsNullOrEmpty())
            return NotFound(new { message = $"Store with name {name} not found." });
        return Ok(stores);
    }
    
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var stores = await _service.GetAllStoresAsync(cancellationToken);
        return Ok(stores);
    }
    
    [HttpGet("sorted-by-rating/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllSortedByRating(bool sortAsc, CancellationToken cancellationToken)
    {
        var stores = await _service.GetStoresSortedByRatingAsync(sortAsc, cancellationToken);
        return Ok(stores);
    }
    
    [HttpGet("sorted-by-orders-count/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllSortedByOrdersCount(bool sortAsc, CancellationToken cancellationToken)
    {
        var stores = await _service.GetStoresSortedByOrdersCountAsync(sortAsc, cancellationToken);
        return Ok(stores);
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateStoreDto dto, CancellationToken cancellationToken)
    {
        var store = await _service.CreateStoreAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = store.StoreId }, store);
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdateStoreDto dto, CancellationToken cancellationToken)
    {
        var existing = await _service.GetStoreByIdAsync(dto.StoreId, cancellationToken);
        if (existing == null)
            return NotFound(new { message = $"Store with id {dto.StoreId} not found." });
        
        await _service.UpdateStoreAsync(dto, cancellationToken); 
        return NoContent();
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var existing = await _service.GetStoreByIdAsync(id, cancellationToken);
        if (existing == null)
            return NotFound(new { message = $"Store with id {id} not found." });
        
        await _service.DeleteStoreAsync(id, cancellationToken); 
        return NoContent();
    }
}