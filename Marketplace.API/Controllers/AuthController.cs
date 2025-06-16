using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Marketplace.BBL.DTO.User;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;
    private readonly IJwtService _jwtService;
    
    public AuthController(IAuthService authService, IJwtService jwtService, UserManager<User> userManager)
    {
        _authService = authService;
        _jwtService = jwtService;
        _userManager = userManager;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserDto model)
    {
        var (success, error) = await _authService.RegisterUserAsync(model);
        return success ? Ok("User is created!") : BadRequest(error);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Identifier)
                   ?? await _userManager.FindByEmailAsync(model.Identifier)
                   ?? await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.Identifier);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Incorrect credentials!");

        var token = await _jwtService.GenerateToken(user);
        return Ok(new { token });
    }
    
}