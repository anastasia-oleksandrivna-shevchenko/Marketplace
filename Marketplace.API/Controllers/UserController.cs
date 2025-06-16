using System.Security.Claims;
using Marketplace.BBL.DTO.User;
using Marketplace.BBL.Services.Interfaces;
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
    
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _service.GetUserByIdAsync(id);
        if (user == null)
            return NotFound(new { message = $"User with id {id} not found." });
        return Ok(user);
    }
    
    private int GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userId, out var id) ? id : throw new UnauthorizedAccessException("Invalid or missing user ID in token.");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAllUsersAsync();
        return Ok(users);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
    {
        dto.UserId = GetUserId();

        await _service.UpdateUserAsync(dto);
        return Ok(new { message = "User profile updated successfully." });
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        dto.UserId = GetUserId();

        await _service.ChangePasswordAsync(dto);
        return Ok(new { message = "Password changed successfully." });
    }

    [HttpPut("change-email")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto dto)
    {
        dto.UserId = GetUserId();

        await _service.ChangeEmailAsync(dto);
        return Ok(new { message = "Email updated successfully." });
    }

    [HttpPut("change-username")]
    public async Task<IActionResult> ChangeUsername([FromBody] ChangeUsernameDto dto)
    {
        dto.UserId = GetUserId();

        await _service.ChangeUsernameAsync(dto);
        return Ok(new { message = "Username updated successfully." });
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        var userId = GetUserId();
        await _service.DeleteUserAsync(userId); 
        return Ok(new { message = "User account deleted successfully." });
    }

    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteUserAsync(id); 
        return Ok(new { message = $"User with id {id} deleted." });
    }
    
    [HttpGet("by-username/{username}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _service.GetUserByUsernameAsync(username);
        if (user == null)
            return NotFound(new { message = $"User with username '{username}' not found." });
        return Ok(user);
    }
    
    [HttpGet("by-email/{email}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var user = await _service.GetUserByEmailAsync(email);
        if (user == null)
            return NotFound(new { message = $"User with email '{email}' not found." });
        return Ok(user);
    }
    
    [HttpGet("by-role/{role}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsersByRole(string role)
    {
        var users = await _service.GetUsersByRoleAsync(role);
        return Ok(users);
    }
}