using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Marketplace.BBL.DTO.Auth;
using Marketplace.BBL.DTO.User;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mapster;

namespace Marketplace.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    
    public AuthController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto model)
    {
        await _jwtService.RegisterUserAsync(model);
        return Ok(new { Success = true, Error = (string)null! });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var loginModel = await _jwtService.LoginAsync(model);
        var loginResponseDto = loginModel.Adapt<LoginResponseDto>();
        return Ok(loginResponseDto);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        
        var result = await _jwtService.RefreshTokenAsync(ipAddress);
        
        return Ok(result);
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var result = await _jwtService.ConfirmEmailAsync(userId, token);

        if (!result)
            return BadRequest("Email confirmation failed.");

        return Ok("Email successfully confirmed.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto model)
    {
        var result = await _jwtService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
        if (!result)
            return BadRequest("Failed to reset password.");
        return Ok("Password reset successful.");
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _jwtService.LogoutAsync();
        return Ok(new {message = "Logout successful"});
    }
    
}