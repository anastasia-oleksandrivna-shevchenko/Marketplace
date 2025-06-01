using Marketplace.BBL.DTO.Order;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        return Ok(orders);
    }

    [HttpGet("store/{storeId}")]
    public async Task<IActionResult> GetByStore(int storeId)
    {
        var orders = await _service.GetOrdersByStoreIdAsync(storeId);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _service.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        var order = await _service.CreateOrderAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateOrderStatusDto dto)
    {
        await _service.UpdateOrderAsync(dto); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
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