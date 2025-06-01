namespace DotnetBatchInjection.Core;

[AttributeUsage(AttributeTargets.Class)]
public class AutoBindAttribute(string? appsettingKey = null) : Attribute
{
    public string? AppsettingKey => appsettingKey;
}
