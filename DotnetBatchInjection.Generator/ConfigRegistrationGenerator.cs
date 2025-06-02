using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotnetBatchInjection.SourceGenerator
{
    [Generator]
    public class ConfigRegistrationGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var configs = FindConfigRegistrations(context);
            if (configs.Count > 0)
            {
                var source = GenerateConfigRegistrationSource(configs);
                context.AddSource("ConfigRegistrationExtensions.g.cs", source);
            }
        }

        private List<ConfigRegistrationInfo> FindConfigRegistrations(GeneratorExecutionContext context)
        {
            var configs = new List<ConfigRegistrationInfo>();

            foreach (var classSymbol in GeneratorHelpers.GetAllClasses(context))
            {
                var attribute = classSymbol.GetAttributes()
                    .FirstOrDefault(ad => ad.AttributeClass?.Name == "AutoBindAttribute");

                if (attribute == null) continue;

                configs.Add(new ConfigRegistrationInfo
                {
                    ClassName = classSymbol.ToDisplayString(),
                    SectionKey = attribute.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? classSymbol.Name
                });
            }

            return configs;
        }

        private string GenerateConfigRegistrationSource(List<ConfigRegistrationInfo> configs)
        {
            var methodBody = new StringBuilder();
            methodBody.AppendLine("            if (configuration == null) return services;");

            foreach (var config in configs)
            {
                methodBody.AppendLine($"            services.Configure<{config.ClassName}>(configuration.GetSection(\"{config.SectionKey}\"));");
            }

            return GeneratorHelpers.GenerateExtensionClass(
                className: "DependencyInjectionExtensions",
                methodName: "RegisterConfigs",
                methodParameters: ", IConfiguration configuration",
                methodBody: methodBody.ToString());
        }
    }
}