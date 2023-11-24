using Microsoft.AspNetCore.Builder;

namespace XoomCore.Services.Middleware;

internal static class Startup
{
    internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>();
}
