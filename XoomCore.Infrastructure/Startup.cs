using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XoomCore.Infrastructure.Caching;
using XoomCore.Infrastructure.Common;
using XoomCore.Infrastructure.OpenApi;
using XoomCore.Infrastructure.SecurityHeaders;
using XoomCore.Persistence;
using XoomCore.Services.Auth;
using XoomCore.Services.Mapper;
using XoomCore.Services.Middleware;

namespace XoomCore.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructures(this IServiceCollection services, IConfiguration configuration)
    {
        //var applicationAssembly = typeof(XoomCore.Application.Startup).GetTypeInfo().Assembly;

        return services
            .AddApiVersioning()
            .AddAuth(configuration)
            .AddMemoryCache()
            .AddCaching()
            .AddPersistence()
            .AddMappers()
            .AddOpenApiDocumentation(configuration)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices();
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });


    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
        builder
            .UseRequestLocalization()
            .UseSecurityHeaders(config)
            //.UseFileStorage()
            .UseExceptionMiddleware()
            .UseRouting()
            //.UseCorsPolicy()
            .UseAuthentication()
            .UseCurrentUser()
            //.UseMultiTenancy()
            .UseAuthorization()
            //.UseRequestLogging(config)
            //.UseHangfireDashboard(config)
            .UseOpenApiDocumentation(config)
            .UseStaticFiles();


    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        //builder.MapHealthCheck();
        //builder.MapNotifications();
        return builder;
    }

    private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapHealthChecks("/api/health");
}
