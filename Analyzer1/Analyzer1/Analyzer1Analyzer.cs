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
        //public const string ClassDiagnosticId = "MissingClassDocumentation";
        public const string MethodDiagnosticId = "MissingMethodDocumentation";

        //public const string DiagnosticId = "Analyzer1";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        //private static readonly string Title = "missing class documentation";
        ////new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        //private static readonly string MessageFormat = "Class '{0}' doesn't have a documentation";
        ////= new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        //private static readonly string Description = "Public class must have a documentation";
        //new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        //private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        //    DiagnosticId,
        //    Title,
        //    MessageFormat,
        //    Category,
        //    DiagnosticSeverity.Error,
        //    true,
        //    Description);


        //private const string ClassTitle = "Missing class documentation";
        //private const string ClassMessageFormat = "Class '{0}' doesn't have a documentation";
        //private const string ClassDescription = "Public class must have a documentation";

        private const string MethodTitle = "Missing method documentation";
        private const string MethodMessageFormat = "Method '{0}' doesn't have a documentation";
        private const string MethodDescription = "Public method must have a documentation";

        //private static readonly DiagnosticDescriptor ClassRule = new DiagnosticDescriptor(
        //    ClassDiagnosticId, ClassTitle, ClassMessageFormat,
        //    Category, DiagnosticSeverity.Error, true, ClassDescription);

        private static readonly DiagnosticDescriptor MethodRule = new DiagnosticDescriptor(
            MethodDiagnosticId, MethodTitle, MethodMessageFormat,
            Category, DiagnosticSeverity.Error, true, MethodDescription);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(MethodRule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);

            //context.RegisterSymbolAction(AnalyzeClass, SymbolKind.NamedType);
            //context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);


            //context.RegisterSymbolAction(AnalyzeNamespace, SymbolKind.Namespace);

            //context.RegisterSyntaxTreeAction(AnalyzeTree);
        }

        //private static void AnalyzeClass(SymbolAnalysisContext context)
        //{
        //    if (string.IsNullOrWhiteSpace(context.Symbol.GetDocumentationCommentXml()))
        //    {
        //        var diagnostic = Diagnostic.Create(ClassRule, context.Symbol.Locations.FirstOrDefault(), context.Symbol.Name);
        //        context.ReportDiagnostic(diagnostic);
        //    }

        //    //Debugger.Launch();

        //    //var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

        //    //var methods = namedTypeSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Method && !m.IsImplicitlyDeclared);
        //    //foreach (var method in methods)
        //    //{
        //    //    if (string.IsNullOrEmpty(method.ContainingSymbol.GetDocumentationCommentXml()))
        //    //    {
        //    //        var diagnostic = Diagnostic.Create(MethodRule, method.Locations.FirstOrDefault(), method.Name);
        //    //        context.ReportDiagnostic(diagnostic);
        //    //    }
        //    //}
        //}

        //private static void AnalyzeMethod(SymbolAnalysisContext context)
        //{
        //    if (string.IsNullOrEmpty(context.Symbol.GetDocumentationCommentXml()))
        //    {
        //        var diagnostic = Diagnostic.Create(MethodRule, context.Symbol.Locations.FirstOrDefault(), context.Symbol.Name);
        //        context.ReportDiagnostic(diagnostic);
        //    }
        //}



        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            if (string.IsNullOrEmpty(context.Symbol.GetDocumentationCommentXml()))
            {
                var diagnostic = Diagnostic.Create(MethodRule, context.Symbol.Locations.FirstOrDefault(), context.Symbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }


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
