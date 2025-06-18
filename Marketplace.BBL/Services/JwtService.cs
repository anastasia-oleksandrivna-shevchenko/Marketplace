using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Marketplace.BBL.DTO.Auth;
using Marketplace.BBL.DTO.User;
using Marketplace.BBL.Exceptions;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Marketplace.BBL.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailService _emailService;

    public JwtService(IConfiguration configuration, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
    }
    
    public async Task RegisterUserAsync(CreateUserDto model)
    {
        if (await _userManager.FindByNameAsync(model.Username) != null)
            throw new ConflictException("User with such name already exists.");
        if (await _userManager.FindByEmailAsync(model.Email) != null)
            throw new ConflictException("User with such email already exists.");
        if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.Phone))
            throw new ConflictException("User with such phone number already exists.");

        var user = new User
        {
            UserName = model.Username,
            Email = model.Email,
            PhoneNumber = model.Phone,
            FirstName = model.FirstName,
            LastName = model.LastName,
            MiddleName = model.MiddleName,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (!result.Succeeded)
            throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.Description)));

        if (!string.IsNullOrEmpty(model.Role))
        {
            var allowedRoles = new[] { "Admin", "Buyer", "Seller" };

            if (!allowedRoles.Contains(model.Role))
                throw new ArgumentException("Invalid role specified.");

            await _userManager.AddToRoleAsync(user, model.Role);
        }
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = Uri.EscapeDataString(token);
        
        var apiBaseUrl = _configuration["ApiBaseUrl"]; 
        var confirmationLink = $"{apiBaseUrl}/api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

        await _emailService.SendEmailAsync(
            model.Email, 
            "Confirm your email", 
            confirmationLink
        );
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName!);
        if(user == null || !await _userManager.CheckPasswordAsync(user, model.Password!))
            throw new JwtUnauthorizedException("Invalid username or password");
        
        if (!user.EmailConfirmed)
            throw new JwtUnauthorizedException("Email is not confirmed.");
        
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = GenerateJwtToken(user, roles);
        var expiresIn = GetTokenValiditySeconds();
        
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        
        var refreshToken = GenerateRefreshToken(ipAddress);
        refreshToken.UserId = user.Id;
        user.RefreshTokens.Add(refreshToken);
        
        CleanOldRefreshTokens(user);
        await _userManager.UpdateAsync(user);
        
        SetRefreshTokenCookie(refreshToken.Token, refreshToken.Expires);

        return new LoginResponseDto
        {
            UserName = user.UserName,
            Email = user.Email,
            AccessToken = accessToken,
            ExpiresIn = expiresIn
        };
    }

    public async Task<RefreshTokenResponseDto> RefreshTokenAsync(string ipAddress)
    {
        var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            throw new JwtTokenMissingException();
        
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

        if (user == null)
            throw new JwtTokenInvalidException();
        
        var token = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken)
            ?? throw new JwtTokenInvalidException();
        
        if(!token.IsActive)
            throw new JwtTokenExpiredException();
        
        token.Revoked = DateTime.Now;
        token.RevokedByIp = ipAddress;
        
        var newRefreshToken = GenerateRefreshToken(ipAddress);
        token.ReplacedByToken = newRefreshToken.Token;
        user.RefreshTokens.Add(newRefreshToken);
        
        CleanOldRefreshTokens(user);
        await _userManager.UpdateAsync(user);
        
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = GenerateJwtToken(user, roles);
        
        SetRefreshTokenCookie(newRefreshToken.Token, newRefreshToken.Expires);

        return new RefreshTokenResponseDto
        {
            AccessToken = accessToken,
            ExpiresIn = GetTokenValiditySeconds(),
        };
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new JwtUnauthorizedException("User not found");
        
        var decoded = Uri.UnescapeDataString(token);
        var result = await _userManager.ConfirmEmailAsync(user, decoded);
        return result.Succeeded;
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var encodedToken = Uri.EscapeDataString(token);
        
        var message = $"Your password reset code is {encodedToken}\n Use this code along with your email to reset password.";
        
        await _emailService.SendEmailAsync(email, "Reset password", message);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;
        
        var decodedToken = Uri.UnescapeDataString(token);
        
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);
        return result.Succeeded;
    }

    public async Task LogoutAsync()
    {
        var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            throw new JwtTokenMissingException();
        
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
        
        if (user == null)
            throw new JwtTokenInvalidException();

        var token = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
        if(!token.IsActive)
            throw new JwtTokenExpiredException();
        
        token.Revoked = DateTime.Now;
        token.RevokedByIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        
        await _userManager.UpdateAsync(user);
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
        
    }
    

    private string GenerateJwtToken(User user, IList<string> roles)
    {
        var claims = GenerateClaims(user, roles);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(GetTokenValidityMinutes()),
            Issuer = _configuration["JwtConfig:Issuer"],
            Audience = _configuration["JwtConfig:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]!)),
                SecurityAlgorithms.HmacSha512Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    private static IEnumerable<Claim> GenerateClaims(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }
    
    private int GetTokenValidityMinutes()
        => int.TryParse(_configuration["JwtConfig:TokenValidityMins"], out var mins) ? mins : 30;

    private int GetTokenValiditySeconds()
        => GetTokenValidityMinutes() * 60;

    private static RefreshToken GenerateRefreshToken(string ipAddress)
    {
        var randonBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randonBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randonBytes),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }

    private void CleanOldRefreshTokens(User user, int maxActiveTokens = 5)
    {
        var activeTokens = user.RefreshTokens
            .Where(t => t.IsActive)
            .OrderByDescending(t => t.Created)
            .ToList();

        if (activeTokens.Count <= maxActiveTokens)
            return;

        var tokensToRemove = activeTokens.Skip(maxActiveTokens).ToList();
        foreach (var token in tokensToRemove)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = "auto-cleanup";
        }
    }
    
    private void SetRefreshTokenCookie(string token, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,   
            Secure = true,
            SameSite = SameSiteMode.Strict, 
            Expires = expires
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}
