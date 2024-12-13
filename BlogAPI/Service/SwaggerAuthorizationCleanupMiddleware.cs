namespace BlogAPI.Services;

public class SwaggerAuthorizationCleanupMiddleware
{
    private readonly RequestDelegate _next;

    public SwaggerAuthorizationCleanupMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/users/logout", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Cookies.Delete("swagger_auth_token");
        }

        await _next(context);
    }
}