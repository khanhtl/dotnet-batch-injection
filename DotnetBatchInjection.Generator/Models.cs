using System.Collections.Generic;

namespace DotnetBatchInjection.SourceGenerator
{
    internal class ServiceRegistrationInfo
    {
        public string ClassName { get; set; }
        public string? InterfaceName { get; set; }
        public string Lifetime { get; set; }
    }

    internal class ConfigRegistrationInfo
    {
        public string ClassName { get; set; }
        public string SectionKey { get; set; }
    }

    internal static class ServiceLifetimeNames
    {
        private static readonly Dictionary<ServiceLifetime, string> LifetimeMap = new()
        {
            [ServiceLifetime.Singleton] = "Singleton",
            [ServiceLifetime.Scoped] = "Scoped",
            [ServiceLifetime.Transient] = "Transient"
        };

        public static string GetName(ServiceLifetime lifetime) => LifetimeMap[lifetime];
    }
}