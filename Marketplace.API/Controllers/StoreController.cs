using Marketplace.BBL.DTO.Store;
using Marketplace.BBL.Services.Interfaces;
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
    public async Task<IActionResult> GetById(int id)
    {
        var store = await _service.GetStoreByIdAsync(id);
        if (store == null)
            return NotFound(new { message = $"Store with id {id} not found." });
        return Ok(store);
    }

    [HttpGet("by-name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var stores = await _service.GetStoresByNameAsync(name);
        if (stores.IsNullOrEmpty())
            return NotFound(new { message = $"Store with name {name} not found." });
        return Ok(stores);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var stores = await _service.GetAllStoresAsync();
        return Ok(stores);
    }
    
    [HttpGet("sorted-by-rating/{sort}")]
    public async Task<IActionResult> GetAllSortedByRating(bool sortAsc)
    {
        var stores = await _service.GetStoresSortedByRatingAsync(sortAsc);
        return Ok(stores);
    }
    
    [HttpGet("sorted-by-orders-count/{sort}")]
    public async Task<IActionResult> GetAllSortedByOrdersCount(bool sortAsc)
    {
        var stores = await _service.GetStoresSortedByOrdersCountAsync(sortAsc);
        return Ok(stores);
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateStoreDto dto)
    {
        var store = await _service.CreateStoreAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = store.StoreId }, store);
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateStoreDto dto)
    {
        var existing = await _service.GetStoreByIdAsync(dto.StoreId);
        if (existing == null)
            return NotFound(new { message = $"Store with id {dto.StoreId} not found." });
        
        await _service.UpdateStoreAsync(dto); 
        return NoContent();
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _service.GetStoreByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Store with id {id} not found." });
        
        await _service.DeleteStoreAsync(id); 
        return NoContent();
    }
}