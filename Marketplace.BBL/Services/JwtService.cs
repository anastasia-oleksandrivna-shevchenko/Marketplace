using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Marketplace.BBL.Exceptions;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.BBL.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public JwtService(IConfiguration configuration, UserManager<User> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<string> GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        
        var secret = jwtSettings["Secret"];
        if (string.IsNullOrEmpty(secret))
            throw new JwtUnauthorizedException("JWT Secret key is missing in configuration.");
        
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationStr = jwtSettings["ExpirationMinutes"];
        
        if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(expirationStr))
            throw new JwtUnauthorizedException("JWT configuration is incomplete.");

        if (!int.TryParse(expirationStr, out int expirationMinutes))
            throw new JwtUnauthorizedException("JWT expiration time is invalid.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault();
        
        if (string.IsNullOrEmpty(role))
            throw new JwtUnauthorizedException("User does not have an assigned role.");
        
        claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtSettings["ExpirationMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
