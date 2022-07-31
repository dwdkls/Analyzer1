using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer1
{
    // TODO: limit to controllers (ControllerBase & [ApiController])
    // TODO: add classes & records which are used by controllers
    // TODO: split member names into sentences
    // TODO: return types 
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
            var type = (INamedTypeSymbol)context.Symbol;

            var isController = type.BaseType.Name == "ControllerBase" 
                || type.GetAttributes().Any(q => q.GetType().Name == "ApiControllerAttribute");

            if (isController)
            {
                if (context.Symbol.DeclaredAccessibility == Accessibility.Public
                    && string.IsNullOrWhiteSpace(context.Symbol.GetDocumentationCommentXml()))
                {
                    var diagnostic = Diagnostic.Create(DiagnosticRule, context.Symbol.Locations.FirstOrDefault(), context.Symbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static void AnalyzeProperty(SymbolAnalysisContext context)
        {
            if (context.Symbol.DeclaredAccessibility == Accessibility.Public
                && string.IsNullOrWhiteSpace(context.Symbol.GetDocumentationCommentXml()))
            {
                var diagnostic = Diagnostic.Create(DiagnosticRule, context.Symbol.Locations.FirstOrDefault(), context.Symbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void AnalyzeMethod(SymbolAnalysisContext context)
        {
            var method = context.Symbol as IMethodSymbol;

            if (method.MethodKind == MethodKind.Ordinary)
            {
                AnalyzeSymbol(context);
            }
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
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
