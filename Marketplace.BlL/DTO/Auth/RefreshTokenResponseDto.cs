namespace Marketplace.BLL.DTO.Auth;

public class RefreshTokenResponseDto
{
    public string AccessToken { get; set; } = null!;
    public int ExpiresIn { get; set; }
}