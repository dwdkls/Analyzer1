using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer1
{
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

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
            context.RegisterSymbolAction(AnalyzeProperty, SymbolKind.Property);
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

        private static void AnalyzeMethod(SymbolAnalysisContext context)
        {
            var method = (IMethodSymbol)context.Symbol;

            if (method.MethodKind == MethodKind.Ordinary)
            {
                AnalyzeSymbol(context);
            }
        }

        private static void AnalyzeProperty(SymbolAnalysisContext context)
        {
            if (context.Symbol.DeclaredAccessibility == Accessibility.Public)
            {
                if (string.IsNullOrEmpty(context.Symbol.GetDocumentationCommentXml()))
                {
                    var diagnostic = Diagnostic.Create(DiagnosticRule, context.Symbol.Locations.FirstOrDefault(), context.Symbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
