using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CGym.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception");

                var (status, title, detail) = ex switch
                {
                    ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request", ex.Message),
                    KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found", ex.Message),
                    InvalidOperationException => (StatusCodes.Status409Conflict, "Conflict", ex.Message),
                    _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "Der skete en uventet fejl.")
                };

                var problem = new ProblemDetails 
                {
                    Status = status,
                    Title = title,
                    Detail = detail,
                    Type = $"https://httpstatuses.com/{status}"
                };

                context.Response.StatusCode = status;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}