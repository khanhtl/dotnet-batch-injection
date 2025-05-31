namespace DotnetBatchInjection.Core;

public static class DependencyInjectionExtensions
{
    public static void RegisterServices(
        this IServiceCollection services,
        string serviceSubix = "Service",
        string interfacePrefix = "I")
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var allTypes = Assembly.GetExecutingAssembly().GetTypes();

        var implementationTypes = allTypes
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(serviceSubix))
            .ToList();

        foreach (var implementationType in implementationTypes)
        {
            var interfaceType = FindMatchingInterface(allTypes, implementationType, serviceSubix, interfacePrefix);

            if (interfaceType == null)
            {
                throw new InvalidOperationException(
                    $"No matching interface found for {implementationType.Name}. " +
                    $"Expected interface: {interfacePrefix}{implementationType.Name.Substring(serviceSubix.Length)}");
            }

            var lifetime = GetServiceLifetime(implementationType);

            RegisterService(services, interfaceType, implementationType, lifetime);
        }
    }

    private static Type? FindMatchingInterface(Type[] allTypes, Type implementationType, string servicePrefix, string interfacePrefix)
    {
        var interfaceName = interfacePrefix + implementationType.Name;
        return allTypes.FirstOrDefault(t => t.IsInterface && t.Name.Equals(interfaceName, StringComparison.Ordinal));
    }

    private static ServiceLifetime GetServiceLifetime(Type implementationType)
    {
        var attribute = implementationType.GetCustomAttribute<ServiceTypeAttribute>();
        return attribute?.Lifetime ?? ServiceLifetime.Scoped;
    }

    private static void RegisterService(IServiceCollection services, Type interfaceType, Type implementationType, ServiceLifetime lifetime)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(interfaceType, implementationType);
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped(interfaceType, implementationType);
                break;
            case ServiceLifetime.Transient:
                services.AddTransient(interfaceType, implementationType);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(lifetime), $"Unsupported lifetime: {lifetime}");
        }
    }
}