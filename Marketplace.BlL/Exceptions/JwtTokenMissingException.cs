namespace Marketplace.BLL.Exceptions;

public class JwtTokenMissingException() : JwtUnauthorizedException("Refresh token is missing!");