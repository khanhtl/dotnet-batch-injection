namespace DotnetBatchInjection.Core;

public static class DependencyInjectionExtensions
{
    public static void RegisterServices(
        this IServiceCollection services,
        string serviceSuffix = "Service",
        string interfacePrefix = "I")
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var allTypes = Assembly.GetExecutingAssembly().GetTypes();

        var implementationTypes = allTypes
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(serviceSuffix))
            .ToList();

        foreach (var implementationType in implementationTypes)
        {
            var interfaceType = FindMatchingInterface(allTypes, implementationType, serviceSuffix, interfacePrefix);

            if (interfaceType == null)
            {
                throw new InvalidOperationException(
                    $"No matching interface found for {implementationType.Name}. " +
                    $"Expected interface: {interfacePrefix}{implementationType.Name.Substring(serviceSuffix.Length)}");
            }

            var lifetime = GetServiceLifetime(implementationType);

            RegisterService(services, interfaceType, implementationType, lifetime);
        }
    }

    public static void RegisterConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var assembly = Assembly.GetExecutingAssembly();
        var configTypes = assembly.GetTypes()
            .Where(t => t.IsClass && t.GetCustomAttribute<AutoBindAttribute>() != null)
            .ToList();

        foreach (var type in configTypes)
        {
            try
            {
                var attr = type.GetCustomAttribute<AutoBindAttribute>();
                var sectionKey = attr?.AppsettingKey ?? type.Name;
                var section = configuration.GetSection(sectionKey);

                if (!section.Exists())
                {
                    continue;
                }

                var genericConfigure = ConfigureMethod.MakeGenericMethod(type);
                genericConfigure.Invoke(null, new object[] { services, section });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    private static readonly MethodInfo ConfigureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .First(m => m.Name == "Configure"
                    && m.GetGenericArguments().Length == 1
                    && m.GetParameters().Length == 2);

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