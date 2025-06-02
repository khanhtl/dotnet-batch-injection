namespace DotnetBatchInjection.SourceGenerator
{
    internal class ServiceRegistrationInfo
    {
        public string ClassName { get; set; }
        public string InterfaceName { get; set; }
        public string Lifetime { get; set; }
    }

    internal class ConfigRegistrationInfo
    {
        public string ClassName { get; set; }
        public string SectionKey { get; set; }
    }

    internal static class ServiceLifetimeNames
    {
        public const string Singleton = "Singleton";
        public const string Scoped = "Scoped";
        public const string Transient = "Transient";
    }
}