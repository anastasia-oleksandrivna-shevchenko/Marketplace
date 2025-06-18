using Marketplace.BLL.DTO.OrderItem;
using Marketplace.BLL.Services.Interfaces;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ByOrderId(int orderId, CancellationToken cancellationToken)
    {
        var orderitems = await _service.GetItemsByOrderIdAsync(orderId, cancellationToken);
        if (orderitems.IsNullOrEmpty())
            return NotFound(new { message = $"Order items with order id {orderId} not found." });
        
        return Ok(orderitems);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var orderitem = await _service.GetOrderItemByIdAsync(id, cancellationToken);
        if (orderitem == null)
            return NotFound(new { message = $"OrderItem with id {id} not found." });
        
        return Ok(orderitem);
    }
    
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var orderitems = await _service.GetAllOrderItemsAsync(cancellationToken);
        return Ok(orderitems);
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody]CreateOrderItemDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var orderitem = await _service.CreateOrderItemAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = orderitem.OrderItemId }, orderitem);
    }

    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody]UpdateOrderItemDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _service.GetOrderItemByIdAsync(dto.OrderItemId, cancellationToken);
        if (existing == null)
            return NotFound(new { message = $"OrderItem with id {dto.OrderItemId} not found." });
        
        await _service.UpdateOrderItemAsync(dto, cancellationToken); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var existing = await _service.GetOrderItemByIdAsync(id, cancellationToken);
        if (existing == null)
            return NotFound(new { message = $"OrderItem with id {id} not found." });
        
        await _service.DeleteOrderItemAsync(id, cancellationToken); 
        return NoContent();
    }
}