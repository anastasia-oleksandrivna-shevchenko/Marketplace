using Marketplace.BBL.DTO.Order;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Controllers;

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
    public async Task<IActionResult> GetByCustomer(int customerId)
    {
        var orders = await _service.GetOrdersByCustomerIdAsync(customerId);
        if(orders.IsNullOrEmpty())
            return NotFound(new { message = $"Orders with customer id {customerId} not found." });
        
        return Ok(orders);
    }

    [HttpGet("store/{storeId}")]
    public async Task<IActionResult> GetByStore(int storeId)
    {
        var orders = await _service.GetOrdersByStoreIdAsync(storeId);
        if(orders.IsNullOrEmpty())
            return NotFound(new { message = $"Orders with customer id {storeId} not found." });
        
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _service.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound(new { message = $"Order with id {id} not found." });
        return Ok(order);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody]CreateOrderDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var order = await _service.CreateOrderAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody]UpdateOrderStatusDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _service.GetOrderByIdAsync(dto.OrderId);
        if (existing == null)
            return NotFound(new { message = $"Order with id {dto.OrderId} not found." });
        
        await _service.UpdateOrderAsync(dto); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _service.GetOrderByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Order with id {id} not found." });

        await _service.DeleteOrderAsync(id); 
        return NoContent();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _service.GetAllOrdersAsync();
        return Ok(orders);
    }
}