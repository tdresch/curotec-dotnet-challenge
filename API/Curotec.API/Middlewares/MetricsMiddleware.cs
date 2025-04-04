using System.Diagnostics;


namespace Curotec.API.Middlewares
{

    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MetricsMiddleware> _logger;

        public MetricsMiddleware(RequestDelegate next, ILogger<MetricsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                var method = context.Request.Method;
                var path = context.Request.Path;
                var statusCode = context.Response.StatusCode;
                var elapsed = stopwatch.ElapsedMilliseconds;

                _logger.LogInformation(
                    "[Metrics] {Method} {Path} responded {StatusCode} in {Elapsed} ms",
                    method, path, statusCode, elapsed
                );
            }
        }
    }

}