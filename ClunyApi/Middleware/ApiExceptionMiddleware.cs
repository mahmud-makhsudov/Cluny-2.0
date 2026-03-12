using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ClunyApi.Exceptions;

namespace ClunyApi.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;

        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            object payload = new { error = "An unexpected error occurred." };

            switch (ex)
            {
                case EntityNotFoundException e:
                    code = HttpStatusCode.NotFound;
                    payload = new { error = e.Message };
                    break;
                case ArgumentException ae:
                    code = HttpStatusCode.BadRequest;
                    payload = new { error = ae.Message };
                    break;
                case InvalidEntityException ie:
                    code = HttpStatusCode.BadRequest;
                    payload = new { error = ie.Message };
                    break;
            }

            var json = JsonSerializer.Serialize(payload);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(json);
        }
    }
}
