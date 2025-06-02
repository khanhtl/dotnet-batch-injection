using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace DotnetBatchInjection.SourceGenerator
{
    [Generator]
    public class ServiceRegistrationGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var services = FindServiceRegistrations(context);
            if (services.Count == 0) return;

            var source = GenerateServiceRegistrationSource(services);
            context.AddSource(Constants.ServiceRegistrationFileName, source);
        }

        private static List<ServiceRegistrationInfo> FindServiceRegistrations(GeneratorExecutionContext context)
        {
            var services = new List<ServiceRegistrationInfo>();

            foreach (var classSymbol in GeneratorHelpers.GetAllClasses(context))
            {
                if (!IsServiceClass(classSymbol)) continue;

                var interfaceSymbol = FindMatchingInterface(classSymbol);
                if (interfaceSymbol == null) continue;

                services.Add(new ServiceRegistrationInfo
                {
                    ClassName = classSymbol.ToDisplayString(),
                    InterfaceName = interfaceSymbol.ToDisplayString(),
                    Lifetime = GetServiceLifetime(classSymbol)
                });
            }

            return services;
        }

        private static bool IsServiceClass(INamedTypeSymbol classSymbol) =>
            classSymbol.Name.EndsWith("Service") &&
            classSymbol.GetAttributes().Any(attr =>
                attr.AttributeClass?.BaseType?.Name == nameof(ServiceTypeAttribute));

        private static INamedTypeSymbol? FindMatchingInterface(INamedTypeSymbol classSymbol) =>
            classSymbol.Interfaces.FirstOrDefault(i => i.Name == $"I{classSymbol.Name}");

        private static string GetServiceLifetime(INamedTypeSymbol classSymbol)
        {
            var attribute = classSymbol.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.BaseType?.Name == nameof(ServiceTypeAttribute));

            return attribute?.ConstructorArguments.FirstOrDefault().Value switch
            {
                ServiceLifetime lifetime => ServiceLifetimeNames.GetName(lifetime),
                _ => ServiceLifetimeNames.GetName(ServiceLifetime.Scoped) // Default to Scoped
            };
        }

        private static string GenerateServiceRegistrationSource(List<ServiceRegistrationInfo> services)
        {
            var methodBody = string.Join("\n", services
                .Select(s => $"            services.Add{s.Lifetime}<{s.InterfaceName}, {s.ClassName}>();"));

            return GeneratorHelpers.GenerateExtensionClass(
                className: Constants.ServiceExtensionClassName,
                methodName: Constants.ServiceExtensionMethodName,
                methodParameters: "",
                methodBody: methodBody);
        }
    }
}