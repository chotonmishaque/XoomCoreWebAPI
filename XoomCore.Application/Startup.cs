namespace XoomCore.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
        .AddCustomValidators(Assembly.GetExecutingAssembly());
    }
}