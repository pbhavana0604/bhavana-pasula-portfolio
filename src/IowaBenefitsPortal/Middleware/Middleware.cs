using System.Text.Json;

namespace IowaBenefitsPortal.Middleware;

/// <summary>
/// Last-resort error handler: returns a generic problem body and logs
/// the detail server-side. Stack traces and exception messages are
/// never sent to the client (a common demo mistake).
/// </summary>
public sealed class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error on {Method} {Path}", context.Request.Method, context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = "https://example.invalid/problems/internal-error",
                title = "Something went wrong.",
                status = 500,
                detail = "Please try again. If the problem continues, use the contact information on the help page.",
            }));
        }
    }
}

/// <summary>Baseline security headers for every response, including static pages.</summary>
public sealed class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        context.Response.Headers["Content-Security-Policy"] =
            "default-src 'self'; style-src 'self'; script-src 'self'; img-src 'self' data:; connect-src 'self'; frame-ancestors 'none'";
        await next(context);
    }
}

/// <summary>
/// Structured request logging without bodies or personal data:
/// method, path, status and duration only.
/// </summary>
public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var started = DateTimeOffset.UtcNow;
        await next(context);
        logger.LogInformation("{Method} {Path} -> {Status} in {Ms}ms",
            context.Request.Method, context.Request.Path, context.Response.StatusCode,
            (DateTimeOffset.UtcNow - started).TotalMilliseconds);
    }
}
