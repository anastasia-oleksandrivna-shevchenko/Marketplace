using Marketplace.BBL.DTO.OrderItem;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemService _service;
    public OrderItemController(IOrderItemService service) => _service = service;

    [HttpGet("by-order/{orderId}")]
    public async Task<IActionResult> ByOrder(int orderId)
    {
        var orderitems = await _service.GetItemsByOrderIdAsync(orderId);
        if (orderitems.IsNullOrEmpty())
            return NotFound(new { message = $"Order items with order id {orderId} not found." });
        
        return Ok(orderitems);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var orderitem = await _service.GetOrderItemByIdAsync(id);
        if (orderitem == null)
            return NotFound(new { message = $"OrderItem with id {id} not found." });
        
        return Ok(orderitem);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var orderitems = await _service.GetAllOrderItemsAsync();
        return Ok(orderitems);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody]CreateOrderItemDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var orderitem = await _service.CreateOrderItemAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = orderitem.OrderItemId }, orderitem);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody]UpdateOrderItemDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _service.GetOrderItemByIdAsync(dto.OrderItemId);
        if (existing == null)
            return NotFound(new { message = $"OrderItem with id {dto.OrderItemId} not found." });
        
        await _service.UpdateOrderItemAsync(dto); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _service.GetOrderItemByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"OrderItem with id {id} not found." });
        
        await _service.DeleteOrderItemAsync(id); 
        return NoContent();
    }
}