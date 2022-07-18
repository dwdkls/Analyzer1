using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Analyzer1
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Analyzer1Analyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Analyzer1";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly string Title = "missing class documentation";
        //new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly string MessageFormat = "Class '{0}' doesn't have a documentation";
        //= new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly string Description = "Public class must have a documentation";
            //new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule
            = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
            //context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);

            //context.RegisterSymbolAction(AnalyzeNamespace, SymbolKind.Namespace);

            //context.RegisterSyntaxTreeAction(AnalyzeTree);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var symbol = context.Symbol;

            var currentDoc = symbol.GetDocumentationCommentXml();

            if (string.IsNullOrEmpty(currentDoc))
            {
                var descriptor = new DiagnosticDescriptor(DiagnosticId,
                    Title,
                    MessageFormat,
                    Category, DiagnosticSeverity.Error, true, Description);

                var diagnostic = Diagnostic.Create(descriptor, symbol.Locations.FirstOrDefault(), symbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }

        //private static void AnalyzeMethod(SymbolAnalysisContext context)
        //{
        //    var namedTypeSymbol = (IMethodSymbol)context.Symbol;

        //    var currentDoc = namedTypeSymbol.GetDocumentationCommentXml();

        //    if (string.IsNullOrEmpty(currentDoc))
        //    {
        //        var descriptor = new DiagnosticDescriptor(DiagnosticId,
        //            "missing class documentation",
        //            "Class '{0}' doesn't have a documentation",
        //            Category, DiagnosticSeverity.Error, true, "Public class must have a documentation");

        //        var diagnostic = Diagnostic.Create(descriptor, namedTypeSymbol.Locations.FirstOrDefault(), namedTypeSymbol.Name);

        //        context.ReportDiagnostic(diagnostic);
        //    }
        //}

        //private void AnalyzeTree(SyntaxTreeAnalysisContext context)
        //{
        //    var filePath = context.Tree.FilePath;
        //}

        //private static void AnalyzeNamespace(SymbolAnalysisContext context)
        //{
        //    var descriptor = new DiagnosticDescriptor(DiagnosticId, "wrong namespace", "Namespace '{0}' is in wrong file",
        //        Category, DiagnosticSeverity.Error, true, "Namespace should be in a valid file");

        //    var space = (INamespaceSymbol)context.Symbol;


        //    // 
        //    if (space.Name != context.Options.ToString())
        //    {
        //        var diag = Diagnostic.Create(descriptor, space.Locations.FirstOrDefault(), space.Name);

        //        context.ReportDiagnostic(diag);
        //    }
        //}
    }
}
