using Marketplace.BBL.DTO.OrderItem;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers;

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
        return Ok(orderitems);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var orderitem = await _service.GetOrderItemByIdAsync(id);
        if (orderitem == null)
            return NotFound();
        return Ok(orderitem);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var orderitems = await _service.GetAllOrderItemsAsync();
        return Ok(orderitems);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateOrderItemDto dto)
    {
        var orderitem = await _service.CreateOrderItemAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = orderitem.OrderItemId }, orderitem);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateOrderItemDto dto)
    {
        await _service.UpdateOrderItemAsync(dto); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteOrderItemAsync(id); 
        return NoContent();
    }
}