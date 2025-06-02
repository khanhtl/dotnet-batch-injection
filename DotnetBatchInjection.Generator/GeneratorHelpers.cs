using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace DotnetBatchInjection.SourceGenerator
{
    internal static class GeneratorHelpers
    {
        public static IEnumerable<INamedTypeSymbol> GetAllClasses(GeneratorExecutionContext context)
        {
            return context.Compilation.SyntaxTrees
                .SelectMany(tree => tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Select(classDecl => context.Compilation.GetSemanticModel(tree).GetDeclaredSymbol(classDecl))
                    .OfType<INamedTypeSymbol>());
        }

        public static string GenerateExtensionClass(
            string className,
            string methodName,
            string methodParameters,
            string methodBody) =>
            string.Format(
                Constants.ExtensionClassTemplate,
                Constants.Header,
                Constants.UsingStatements,
                Constants.Namespace,
                className,
                methodName,
                methodParameters,
                methodBody.Trim());
    }
}