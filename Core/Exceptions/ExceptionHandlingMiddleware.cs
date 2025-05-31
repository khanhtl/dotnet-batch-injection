using System.Net;
using System.Text.Json;

namespace DotnetBatchInjection.Core.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            ArgumentNullException argEx => new ErrorResponse(
                (int)HttpStatusCode.BadRequest,
                "Invalid input data.",
                argEx.Message
            ),
            KeyNotFoundException keyEx => new ErrorResponse(
                (int)HttpStatusCode.NotFound,
                "Resource not found.",
                keyEx.Message
            ),
            DbUpdateException dbEx => new ErrorResponse(
                (int)HttpStatusCode.BadRequest,
                "Database operation failed.",
                dbEx.InnerException?.Message ?? dbEx.Message
            ),
            UnauthorizedAccessException authEx => new ErrorResponse(
                (int)HttpStatusCode.Unauthorized,
                "Unauthorized access.",
                authEx.Message
            ),
            _ => new ErrorResponse(
                (int)HttpStatusCode.InternalServerError,
                "An unexpected error occurred.",
                exception.Message
            )
        };

        _logger.LogError(exception, "Error occurred: {Message}. Details: {Details}",
            errorResponse.Message, errorResponse.Details);

        response.StatusCode = errorResponse.StatusCode;
        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result);
    }
}
