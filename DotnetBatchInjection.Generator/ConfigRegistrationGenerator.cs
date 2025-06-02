using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace DotnetBatchInjection.SourceGenerator
{
    [Generator]
    public class ConfigRegistrationGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var configs = FindConfigRegistrations(context);
            if (configs.Count == 0) return;

            var source = GenerateConfigRegistrationSource(configs);
            context.AddSource(Constants.ConfigRegistrationFileName, source);
        }

        private static List<ConfigRegistrationInfo> FindConfigRegistrations(GeneratorExecutionContext context) =>
            GeneratorHelpers.GetAllClasses(context)
                .Select(classSymbol => (ClassSymbol: classSymbol, Attribute: classSymbol.GetAttributes()
                    .FirstOrDefault(attr => attr.AttributeClass?.Name == nameof(AutoBindAttribute))))
                .Where(tuple => tuple.Attribute != null)
                .Select(tuple => new ConfigRegistrationInfo
                {
                    ClassName = tuple.ClassSymbol.ToDisplayString(),
                    SectionKey = tuple.Attribute.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? tuple.ClassSymbol.Name
                })
                .ToList();

        private static string GenerateConfigRegistrationSource(List<ConfigRegistrationInfo> configs)
        {
            var methodBodyLines = new List<string> { Constants.ConfigNullCheck };
            methodBodyLines.AddRange(configs.Select(config =>
                $"            services.Configure<{config.ClassName}>(configuration.GetSection(\"{config.SectionKey}\"));"));

            var methodBody = string.Join("\n", methodBodyLines);

            return GeneratorHelpers.GenerateExtensionClass(
                className: Constants.ConfigExtensionClassName,
                methodName: Constants.ConfigExtensionMethodName,
                methodParameters: ", IConfiguration configuration",
                methodBody: methodBody);
        }
    }
}