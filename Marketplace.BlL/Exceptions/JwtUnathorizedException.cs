namespace Marketplace.BLL.Exceptions;

public class JwtUnauthorizedException(string message) : UnauthorizedAccessException(message);