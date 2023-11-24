namespace XoomCore.Persistence;

public static class Startup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        return services
            .AddDbContext()
            .AddRepositories()
            .AddUnitOfWork();
    }
}
