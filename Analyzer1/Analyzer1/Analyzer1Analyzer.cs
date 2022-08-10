using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer1
{
    // TODO: limit to controllers (ControllerBase & [ApiController])
    // TODO: add classes & records which are used by controllers
    // TODO: add support for generic parameters
    // TODO: try to create tests
    // TODO: publish as a package

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Analyzer1Analyzer : DiagnosticAnalyzer
    {
        public const string MethodDiagnosticId = "DK001";
        private const string Category = "Naming";
        private const string Title = "Missing member documentation";
        private const string MessageFormat = "Member '{0}' doesn't have a documentation";
        private const string Description = "Public member must have a documentation";

        private static readonly DiagnosticDescriptor DiagnosticRule = new DiagnosticDescriptor(
            MethodDiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(DiagnosticRule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(AnalyzeClass, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeProperty, SymbolKind.Property);
            context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
        }

        private static void AnalyzeClass(SymbolAnalysisContext context)
        {
            //Debugger.Launch();

            if (IsControllerType((INamedTypeSymbol)context.Symbol))
            {
                ReportMissingDocumentationForPublicSymbol(context);
            }
        }

        private static void AnalyzeProperty(SymbolAnalysisContext context)
        {
            //Debugger.Launch();

            if (IsControllerType(context.Symbol.ContainingType))
            {
                ReportMissingDocumentationForPublicSymbol(context);
            }
        }

        private static void AnalyzeMethod(SymbolAnalysisContext context)
        {
            //Debugger.Launch();

            if (IsControllerType(context.Symbol.ContainingType)
                && context.Symbol is IMethodSymbol method
                && method.MethodKind == MethodKind.Ordinary)
            {
                ReportMissingDocumentationForPublicSymbol(context);
            }
        }

        private static bool IsControllerType(INamedTypeSymbol typeSymbol)
        {
            return typeSymbol.BaseType.Name == "ControllerBase"
               || typeSymbol.GetAttributes().Any(q => q.GetType().Name == "ApiControllerAttribute");
        }

        private static void ReportMissingDocumentationForPublicSymbol(SymbolAnalysisContext context)
        {
            if (context.Symbol.DeclaredAccessibility == Accessibility.Public
                && string.IsNullOrWhiteSpace(context.Symbol.GetDocumentationCommentXml()))
            {
                var diagnostic = Diagnostic.Create(DiagnosticRule, context.Symbol.Locations.FirstOrDefault(), context.Symbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
