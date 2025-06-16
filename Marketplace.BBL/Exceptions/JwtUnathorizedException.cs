namespace Marketplace.BBL.Exceptions;

public class JwtUnauthorizedException(string message) : UnauthorizedAccessException(message);