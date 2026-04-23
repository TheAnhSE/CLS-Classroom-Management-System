using System.Net;
using System.Text.Json;
using CLS.BackendAPI.Exceptions;

namespace CLS.BackendAPI.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "Internal Server Error. Please contact support.";
        object data = null;

        switch (exception)
        {
            case NotFoundException notFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = notFoundException.Message;
                break;
            case ConflictException conflictException:
                statusCode = (int)HttpStatusCode.Conflict;
                message = conflictException.Message;
                break;
            case ValidationException validationException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = validationException.Message;
                data = validationException.Errors;
                break;
            case SlaViolationException slaViolationException:
                statusCode = (int)HttpStatusCode.BadRequest; // Or another specific code based on rule
                message = slaViolationException.Message;
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = new 
        { 
            Message = message, 
            StatusCode = statusCode,
            Errors = data 
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(response, jsonOptions);

        return context.Response.WriteAsync(result);
    }
}
