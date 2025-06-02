namespace DotnetBatchInjection.Core;

[AttributeUsage(AttributeTargets.Class)]
public class AutoBindAttribute(string? appsettingKey = null) : Attribute
{
    public string? AppsettingKey => appsettingKey;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ServiceTypeAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }
    public ServiceTypeAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}

public class SingletonServiceAttribute : ServiceTypeAttribute
{
    public SingletonServiceAttribute() : base(ServiceLifetime.Singleton) { }
}

public class ScopedServiceAttribute : ServiceTypeAttribute
{
    public ScopedServiceAttribute() : base(ServiceLifetime.Scoped) { }
}

public class TransientServiceAttribute : ServiceTypeAttribute
{
    public TransientServiceAttribute() : base(ServiceLifetime.Transient) { }
}

public enum ServiceLifetime
{
    Singleton,
    Scoped,
    Transient
}