using Marketplace.BBL.DTO.Order;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    } 

    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByCustomer(int customerId)
    {
        var orders = await _service.GetOrdersByCustomerIdAsync(customerId);
        if(orders.IsNullOrEmpty())
            return NotFound(new { message = $"Orders with customer id {customerId} not found." });
        
        return Ok(orders);
    }

    [HttpGet("store/{storeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByStore(int storeId)
    {
        var orders = await _service.GetOrdersByStoreIdAsync(storeId);
        if(orders.IsNullOrEmpty())
            return NotFound(new { message = $"Orders with customer id {storeId} not found." });
        
        return Ok(orders);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _service.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound(new { message = $"Order with id {id} not found." });
        return Ok(order);
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody]CreateOrderDto dto)
    {
        var order = await _service.CreateOrderAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody]UpdateOrderStatusDto dto)
    {
        var existing = await _service.GetOrderByIdAsync(dto.OrderId);
        if (existing == null)
            return NotFound(new { message = $"Order with id {dto.OrderId} not found." });
        
        await _service.UpdateOrderAsync(dto); 
        return NoContent();
    }

    [Authorize(Roles = "Admin, Seller")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _service.GetOrderByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Order with id {id} not found." });

        await _service.DeleteOrderAsync(id); 
        return NoContent();
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _service.GetAllOrdersAsync();
        return Ok(orders);
    }
}