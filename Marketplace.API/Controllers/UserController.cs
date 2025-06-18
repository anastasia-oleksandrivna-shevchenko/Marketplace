using System.Security.Claims;
using Marketplace.BLL.DTO.User;
using Marketplace.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService userService)
    {
        _service = userService;
    }
    
    private int GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userId, out var id) ? id : throw new UnauthorizedAccessException("Invalid or missing user ID in token.");
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
    {
        var user = await _service.GetUserByIdAsync(id, cancellationToken);
        if (user == null)
            return NotFound(new { message = $"User with id {id} not found." });
        return Ok(user);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await _service.GetAllUsersAsync(cancellationToken);
        return Ok(users);
    }
    
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        dto.UserId = GetUserId();

        await _service.UpdateUserAsync(dto, cancellationToken);
        return Ok(new { message = "User profile updated successfully." });
    }
    
    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        await _service.DeleteUserAsync(userId, cancellationToken); 
        return Ok(new { message = "User account deleted successfully." });
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _service.DeleteUserAsync(id, cancellationToken); 
        return Ok(new { message = $"User with id {id} deleted." });
    }
    
    [HttpGet("by-username/{username}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _service.GetUserByUsernameAsync(username);
        if (user == null)
            return NotFound(new { message = $"User with username '{username}' not found." });
        return Ok(user);
    }
    
    [HttpGet("by-email/{email}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var user = await _service.GetUserByEmailAsync(email);
        if (user == null)
            return NotFound(new { message = $"User with email '{email}' not found." });
        return Ok(user);
    }
    
    [HttpGet("by-role/{role}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsersByRole(string role)
    {
        var users = await _service.GetUsersByRoleAsync(role);
        return Ok(users);
    }
}