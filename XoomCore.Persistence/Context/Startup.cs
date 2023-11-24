using Microsoft.Extensions.Options;
using XoomCore.Persistence.Context.Config;

namespace XoomCore.Persistence.Context;

internal static class Startup
{
    private static readonly ILogger _logger = Log.ForContext(typeof(Startup));

    internal static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.ConfigureDatabase();

        return services.AddDbContext<ApplicationDbContext>((provider, optionsBuilder) =>
        {
            var databaseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            optionsBuilder.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
        });
    }
    private static IServiceCollection ConfigureDatabase(this IServiceCollection services)
    {
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(nameof(DatabaseSettings))
            .PostConfigure(databaseSettings =>
            {
                _logger.Information("Current DB Provider: {dbProvider}", databaseSettings.DBProvider);
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services;
    }
    private static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
    {
        return dbProvider.ToLowerInvariant() switch
        {
            DatabaseProviders.SqlServer => builder.UseSqlServer(connectionString, e => e.MigrationsAssembly("XoomCore.Persistence")),
            _ => throw new InvalidOperationException($"Database Provider : {dbProvider} is not supported."),
        };
    }
}
