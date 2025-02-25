using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;


namespace Afisha.Web.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
 
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            var (statusCode, errorMessage) = exception switch
            {
                not null => (HttpStatusCode.BadRequest, "Invalid request"),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                statusCode = response.StatusCode,
                error = errorMessage,
                details = exception?.Message // Убери, если не хочешь показывать детали
            };

            var json = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(json, cancellationToken);

            // Исключение обработано
            return true; 
        }
    }
}