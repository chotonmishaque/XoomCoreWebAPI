namespace XoomCore.Application.Validations;
internal static class Startup
{
    internal static IServiceCollection AddCustomValidators(this IServiceCollection services, Assembly assembly)
    {
        var customValidatorType = typeof(CustomValidator<>);
        var validatorTypes = assembly
            .GetTypes()
            .Where(type => type.BaseType?.IsGenericType == true &&
                           type.BaseType.GetGenericTypeDefinition() == customValidatorType)
            .ToList();

        foreach (var validatorType in validatorTypes)
        {
            // Determine T by looking at the base class's generic type argument
            var entityType = validatorType.BaseType.GenericTypeArguments[0];
            var genericValidatorType = typeof(IValidator<>).MakeGenericType(entityType);

            services.AddService(genericValidatorType, validatorType, ServiceLifetime.Transient);
            //services.AddTransient(genericValidatorType, validatorType);
        }
        return services;
    }

    private static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        lifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
        };
}
