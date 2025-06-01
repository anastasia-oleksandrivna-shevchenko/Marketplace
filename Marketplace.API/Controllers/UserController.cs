using Marketplace.BBL.DTO.User;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService userService)
    {
        _service = userService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _service.GetUserByIdAsync(id);
        if (user == null)
            return NotFound(new { message = $"User with id {id} not found." });
        return Ok(user);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll(int id)
    {
        var users = await _service.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = await _service.RegisterUserAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userExists = await _service.GetUserByIdAsync(dto.UserId);
        if (userExists == null)
            return NotFound(new { message = $"User with id {dto.UserId} not found." });

        await _service.UpdateUserAsync(dto);
        return NoContent();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userExists = await _service.GetUserByIdAsync(dto.UserId);
        if (userExists == null)
            return NotFound(new { message = $"User with id {dto.UserId} not found." });

        await _service.ChangePasswordAsync(dto);
        return NoContent();
    }

    [HttpPut("change-email")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userExists = await _service.GetUserByIdAsync(dto.UserId);
        if (userExists == null)
            return NotFound(new { message = $"User with id {dto.UserId} not found." });

        await _service.ChangeEmailAsync(dto);
        return NoContent();
    }

    [HttpPut("change-username")]
    public async Task<IActionResult> ChangeUsername([FromBody] ChangeUsernameDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userExists = await _service.GetUserByIdAsync(dto.UserId);
        if (userExists == null)
            return NotFound(new { message = $"User with id {dto.UserId} not found." });

        await _service.ChangeUsernameAsync(dto);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteUserAsync(id); 
        return NoContent();
    }
}