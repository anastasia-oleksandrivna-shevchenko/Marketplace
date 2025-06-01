using Marketplace.BBL.DTO.Store;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            return NotFound();
        return Ok(store);
    }

    [HttpGet("by-name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var stores = await _service.GetStoresByNameAsync(name);
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

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateStoreDto dto)
    {
        var store = await _service.CreateStoreAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = store.StoreId }, store);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateStoreDto dto)
    {
        await _service.UpdateStoreAsync(dto); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteStoreAsync(id); 
        return NoContent();
    }
}