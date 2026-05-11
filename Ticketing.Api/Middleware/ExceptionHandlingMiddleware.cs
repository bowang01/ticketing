using System.Net;
using System.Text.Json;
using Ticketing.Application.Exceptions;

namespace Ticketing.Api.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception at {Path}", context.Request.Path);
                if (context.Response.HasStarted) throw;
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json; charset=utf-8";
                // REST or GraphQL requests
                var payload = new
                {
                    code = "internal_error",
                    message = _env.IsDevelopment() ? ex.Message : "An error occurred while the server was processing the request."
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }
    }
}