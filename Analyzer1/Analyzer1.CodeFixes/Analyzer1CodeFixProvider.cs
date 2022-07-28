using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzer1
{

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Analyzer1CodeFixProvider)), Shared]
    public class Analyzer1CodeFixProvider : CodeFixProvider
    {
        private const string CodeFixTitle = "Generate documentation";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(Analyzer1Analyzer.MethodDiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            //var syntaxRoot = await context.Document.GetSyntaxRootAsync();

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            //var classDeclaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();


            var token = root.FindToken(diagnosticSpan.Start).Parent;

            var codeAction = token switch
            {
                TypeDeclarationSyntax cd => CodeAction.Create(CodeFixTitle, c => AddMissingDocumentation(context.Document, cd, c), nameof(CodeFixTitle)),
                MethodDeclarationSyntax md => CodeAction.Create(CodeFixTitle, c => AddMissingDocumentation(context.Document, md, c), nameof(CodeFixTitle)),
                PropertyDeclarationSyntax pd => CodeAction.Create(CodeFixTitle, c => AddMissingDocumentation(context.Document, pd, c), nameof(CodeFixTitle)),
                _ => throw new NotImplementedException(),
            };

            context.RegisterCodeFix(codeAction, diagnostic);

            #region
            //switch (token)
            //{
            //    case TypeDeclarationSyntax classDeclaration:
            //        {
            //            context.RegisterCodeFix(
            //            CodeAction.Create(
            //                CodeFixTitle,
            //                c => AddMissingDocumentation(context.Document, classDeclaration, c),
            //                nameof(CodeFixTitle)),
            //            diagnostic);
            //            break;
            //        }

            //    case MethodDeclarationSyntax methodDeclaration:
            //        {
            //            context.RegisterCodeFix(
            //            CodeAction.Create(
            //                CodeFixTitle,
            //                c => AddMissingDocumentation(context.Document, methodDeclaration, c),
            //                nameof(CodeFixTitle)),
            //            diagnostic);
            //            break;
            //        }
            //}



            //var propertyDeclaration = token as PropertyDeclarationSyntax;
            //if (methodDeclaration != null)
            //{
            //    context.RegisterCodeFix(
            //    CodeAction.Create(
            //        CodeFixTitle,
            //        c => AddMissingDocumentation(context.Document, methodDeclaration, c),
            //        nameof(CodeFixTitle)),
            //    diagnostic);
            //}

            //CodeAction codeAction = CodeAction.Create(
            //        CodeFixTitle,
            //        c => AddMissingDocumentation(context.Document, methodDeclaration, c),
            //        nameof(CodeFixTitle));




            //context.RegisterCodeFix(
            //    CodeAction.Create(
            //        CodeFixTitle,
            //        c => AddMissingDocumentation(context.Document, classDeclaration, c),
            //        //c => AddMissingDocumentation(context.Document, methodDeclaration, c),
            //        nameof(CodeFixTitle)),
            //    diagnostic);

            /* foreach (var diagnostic in context.Diagnostics)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        ReadabilityResources.SA1102CodeFix,
                        cancellationToken => GetTransformedDocumentAsync(context.Document, diagnostic, cancellationToken),
                        nameof(SA1102CodeFixProvider)),
                    diagnostic);
            }

            return SpecializedTasks.CompletedTask;*/
            #endregion
        }

        //private async Task<Document> AddMissingDocumentation(Document document,
        //   MemberDeclarationSyntax declaration,
        //   CancellationToken cancellationToken)
        //{
        //    //Debugger.Launch();

        //    var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        //    // Generate the documentation text

        //    // Get the symbol representing the type to be renamed.
        //    //var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
        //    //var typeSymbol = semanticModel.GetDeclaredSymbol(declaration, cancellationToken);

        //    var testDocumentation = XmlDocumentationGenerator.ForClass(declaration);
        //    //var testDocumentation = XmlDocumentationGenerator.ForMethod(declaration);
        //    var documentationTrivia = SyntaxFactory.Trivia(testDocumentation);
        //    var newDeclaration = declaration.WithLeadingTrivia(documentationTrivia);

        //    var syntaxNode = syntaxRoot.ReplaceNode(declaration, newDeclaration);
        //    return document.WithSyntaxRoot(syntaxNode);
        //}



        private async Task<Document> AddMissingDocumentation(Document document,
            TypeDeclarationSyntax declaration,
            CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var testDocumentation = XmlDocumentationGenerator.ForClass(declaration);

            var documentationTrivia = SyntaxFactory.Trivia(testDocumentation);
            var newDeclaration = declaration.WithLeadingTrivia(documentationTrivia);
            var syntaxNode = syntaxRoot.ReplaceNode(declaration, newDeclaration);
            return document.WithSyntaxRoot(syntaxNode);
        }

        private async Task<Document> AddMissingDocumentation(Document document,
            MethodDeclarationSyntax declaration,
            CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var testDocumentation = XmlDocumentationGenerator.ForMethod(declaration);

            var documentationTrivia = SyntaxFactory.Trivia(testDocumentation);
            var newDeclaration = declaration.WithLeadingTrivia(documentationTrivia);
            var syntaxNode = syntaxRoot.ReplaceNode(declaration, newDeclaration);
            return document.WithSyntaxRoot(syntaxNode);
        }

        private async Task<Document> AddMissingDocumentation(Document document,
           PropertyDeclarationSyntax declaration,
           CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var testDocumentation = XmlDocumentationGenerator.ForProperty(declaration);

            var documentationTrivia = SyntaxFactory.Trivia(testDocumentation);
            var newDeclaration = declaration.WithLeadingTrivia(documentationTrivia);
            var syntaxNode = syntaxRoot.ReplaceNode(declaration, newDeclaration);
            return document.WithSyntaxRoot(syntaxNode);
        }

        //private async Task<Solution> MakeUppercaseAsync(Document document, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken)
        //{
        //    // Compute new uppercase name.
        //    var identifierToken = typeDecl.Identifier;
        //    var newName = identifierToken.Text.ToUpperInvariant();

        //    // Get the symbol representing the type to be renamed.
        //    var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
        //    var typeSymbol = semanticModel.GetDeclaredSymbol(typeDecl, cancellationToken);

        //    // Produce a new solution that has all references to that type renamed, including the declaration.
        //    var originalSolution = document.Project.Solution;
        //    var optionSet = originalSolution.Workspace.Options;
        //    var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

        //    // Return the new solution with the now-uppercase type name.
        //    return newSolution;
        //}
    }
}
