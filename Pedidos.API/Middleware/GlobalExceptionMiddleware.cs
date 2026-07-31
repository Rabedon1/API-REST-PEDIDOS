using System.Net;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace Pedidos.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger logger)
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
            _logger.Error(ex, "Ha ocurrido un error no controlado.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        // Asignamos StatusCode 400 si es error de validación o lógica, sino 500
        context.Response.StatusCode = exception is ArgumentException || exception is InvalidOperationException
            ? (int)HttpStatusCode.BadRequest
            : (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            Detalle = context.Response.StatusCode == 500 ? "Error interno del servidor" : "Error de validación"
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}
