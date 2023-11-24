using Serilog;
using XoomCore.Application;
using XoomCore.Infrastructure;
using XoomCore.Infrastructure.Logging.Serilog;
using XoomCore.WebAPI.Configurations;

Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddConfigurations().RegisterSerilog();
    builder.Services.AddControllers();
    builder.Services.AddInfrastructures(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    //await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration);
    app.MapEndpoints();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}
