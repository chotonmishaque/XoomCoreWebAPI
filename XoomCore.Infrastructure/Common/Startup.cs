﻿using Microsoft.Extensions.DependencyInjection;

namespace XoomCore.Infrastructure.Common;

internal static class Startup
{
    internal static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.RegisterImplementations(typeof(ITransientService), ServiceLifetime.Transient);
        return services;
    }
    private static IServiceCollection RegisterImplementations(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
    {
        var implementationTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => interfaceType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
            .Select(type => new { Interface = type.GetInterfaces().FirstOrDefault(), Implementation = type })
            .Where(type => type.Interface != null && interfaceType.IsAssignableFrom(type.Interface));

        foreach (var implementationType in implementationTypes)
            services.RegisterService(implementationType.Interface!, implementationType.Implementation, lifetime);

        return services;
    }

    private static IServiceCollection RegisterService(this IServiceCollection services, Type interfaceType, Type implementationType, ServiceLifetime lifetime)
    {
        return lifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(interfaceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(interfaceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(interfaceType, implementationType),
            _ => throw new ArgumentException("Invalid lifetime specified", nameof(lifetime))
        };
    }
}
