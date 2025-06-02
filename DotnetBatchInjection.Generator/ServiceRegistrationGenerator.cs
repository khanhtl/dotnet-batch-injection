using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotnetBatchInjection.SourceGenerator
{
    [Generator]
    public class ServiceRegistrationGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var services = FindServiceRegistrations(context);
            if (services.Count > 0)
            {
                var source = GenerateServiceRegistrationSource(services);
                context.AddSource("ServiceRegistrationExtensions.g.cs", source);
            }
        }

        private List<ServiceRegistrationInfo> FindServiceRegistrations(GeneratorExecutionContext context)
        {
            var services = new List<ServiceRegistrationInfo>();

            foreach (var classSymbol in GeneratorHelpers.GetAllClasses(context))
            {
                if (!IsServiceClass(classSymbol)) continue;

                services.Add(new ServiceRegistrationInfo
                {
                    ClassName = classSymbol.ToDisplayString(),
                    InterfaceName = FindMatchingInterface(classSymbol)?.ToDisplayString(),
                    Lifetime = GetServiceLifetime(classSymbol)
                });
            }

            return services;
        }

        private bool IsServiceClass(INamedTypeSymbol classSymbol)
        {
            return classSymbol.Name.EndsWith("Service") &&
                   classSymbol.GetAttributes()
                       .Any(ad => ad.AttributeClass?.BaseType?.Name == "ServiceTypeAttribute");
        }

        private INamedTypeSymbol FindMatchingInterface(INamedTypeSymbol classSymbol)
        {
            var interfaceName = $"I{classSymbol.Name}";
            return classSymbol.Interfaces.FirstOrDefault(i => i.Name == interfaceName);
        }

        private string GetServiceLifetime(INamedTypeSymbol classSymbol)
        {
            var attribute = classSymbol.GetAttributes()
                .FirstOrDefault(ad => ad.AttributeClass?.BaseType?.Name == "ServiceTypeAttribute");

            if (attribute?.AttributeClass?.Name == "SingletonServiceAttribute")
                return ServiceLifetimeNames.Singleton;
            if (attribute?.AttributeClass?.Name == "TransientServiceAttribute")
                return ServiceLifetimeNames.Transient;

            return ServiceLifetimeNames.Scoped;
        }

        private string GenerateServiceRegistrationSource(List<ServiceRegistrationInfo> services)
        {
            var methodBody = new StringBuilder();

            foreach (var service in services.Where(s => !string.IsNullOrEmpty(s.InterfaceName)))
            {
                methodBody.AppendLine($"            services.Add{service.Lifetime}<{service.InterfaceName}, {service.ClassName}>();");
            }

            return GeneratorHelpers.GenerateExtensionClass(
                className: "DependencyInjectionExtensions",
                methodName: "RegisterServices",
                methodParameters: "",
                methodBody: methodBody.ToString());
        }
    }
}