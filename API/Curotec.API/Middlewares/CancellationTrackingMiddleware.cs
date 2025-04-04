using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curotec.API.Middlewares
{
    public class CancellationTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CancellationTrackingMiddleware> _logger;

        public CancellationTrackingMiddleware(RequestDelegate next, ILogger<CancellationTrackingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            CancellationToken cancellationToken = context.RequestAborted;
            // Check for cancellation before processing the request
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("Request was cancelled before processing.");
                context.Response.StatusCode = 499; // Custom code indicating client-side cancellation
                return;
            }

            // Continue processing the request
            await _next(context);
        }
    }

}