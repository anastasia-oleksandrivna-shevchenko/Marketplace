using System.Net;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Marketplace.BBL.Exceptions;
using ValidationException = Marketplace.BBL.Exceptions.ValidationException;

namespace Marketplace.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, errorMessage) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message),
            ConflictException => (HttpStatusCode.Conflict, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            ValidationException => (HttpStatusCode.BadRequest, exception.Message),
            JwtTokenMissingException => (HttpStatusCode.Unauthorized,  exception.Message),
            JwtTokenInvalidException => (HttpStatusCode.Unauthorized,  exception.Message),
            JwtTokenExpiredException => (HttpStatusCode.Unauthorized,  exception.Message),
            JwtUnauthorizedException => (HttpStatusCode.Unauthorized,  "Unauthorized access. Please login."),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized,  "Unauthorized access. Please login."),
            _ => (HttpStatusCode.InternalServerError,  "An unexpected error occurred.")
        };

        var response = new
        {
            code = (int)statusCode,
            error = errorMessage,
            details = _env.IsDevelopment() ? exception.StackTrace : null
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
