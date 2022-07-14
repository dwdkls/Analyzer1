using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analyzer1
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Analyzer1CodeFixProvider)), Shared]
    public class Analyzer1CodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(Analyzer1Analyzer.DiagnosticId); }
        }

        //public sealed override FixAllProvider GetFixAllProvider()
        //{
        //    // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        //    return WellKnownFixAllProviders.BatchFixer;
        //}

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            //var syntaxRoot = await context.Document.GetSyntaxRootAsync();

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

            //var act = CodeAction.Create()

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    CodeFixResources.CodeFixTitle,
                    //createChangedSolution: c => MakeUppercaseAsync(context.Document, declaration, c),
                    c => AddMissingDocumentation(context.Document, declaration, c),
                    nameof(CodeFixResources.CodeFixTitle)),
                diagnostic);

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
        }

        private async Task<Document> AddMissingDocumentation(Document document, TypeDeclarationSyntax declaration, CancellationToken cancellationToken)
        {
            //Debugger.Launch();
            
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            // Generate the documentation text
            var identifierToken = declaration.Identifier;
            var text = identifierToken.Text.ToUpperInvariant();

            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var typeSymbol = semanticModel.GetDeclaredSymbol(declaration, cancellationToken);

            var staticKeyWord = SyntaxFactory.Token(SyntaxKind.StaticKeyword).WithTrailingTrivia(SyntaxFactory.Space);
            var newModifiers = declaration.Modifiers.Add(staticKeyWord);
            var newMethodDeclaration = declaration.WithModifiers(newModifiers);

            // Replace the old local declaration with the new local declaration.
            var newRoot = syntaxRoot.ReplaceNode(declaration, newMethodDeclaration);


            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;

            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument;

            #region stuff
            //var x = SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia, new SyntaxList<XmlNodeSyntax>())
            //    .WithLeadingTrivia(SyntaxFactory.DocumentationCommentExterior("/// "))
            //    .WithTrailingTrivia(SyntaxFactory.EndOfLine(text));

            //var xmlSymbol = SyntaxFactory.DocumentationComment(new XmlElementSyntax(new XmlNodeSyntax));

            //            ExpressionStatementSyntax conditionWasTrueInvocation =
            //SyntaxFactory.ExpressionStatement(
            //    SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("LogConditionWasTrue"))
            //    .WithArgumentList(
            //                    SyntaxFactory.ArgumentList()
            //                    .WithOpenParenToken(
            //                        SyntaxFactory.Token(
            //                            SyntaxKind.OpenParenToken))
            //                    .WithCloseParenToken(
            //                        SyntaxFactory.Token(
            //                            SyntaxKind.CloseParenToken))))
            //            .WithSemicolonToken(
            //                SyntaxFactory.Token(
            //                    SyntaxKind.SemicolonToken));

            //var testDocumentation = SyntaxFactory.DocumentationCommentTrivia(
            //    SyntaxKind.SingleLineDocumentationCommentTrivia,
            //    SyntaxFactory.List<XmlNodeSyntax>(
            //        new XmlNodeSyntax[]{
            //            SyntaxFactory.XmlText()
            //            .WithTextTokens(
            //                SyntaxFactory.TokenList(
            //                    SyntaxFactory.XmlTextLiteral(
            //                        SyntaxFactory.TriviaList(
            //                            SyntaxFactory.DocumentationCommentExterior("///")),
            //                        " ",
            //                        " ",
            //                        SyntaxFactory.TriviaList()))),
            //            SyntaxFactory.XmlElement(
            //                SyntaxFactory.XmlElementStartTag(
            //                    SyntaxFactory.XmlName(
            //                        SyntaxFactory.Identifier("summary"))),
            //                SyntaxFactory.XmlElementEndTag(
            //                    SyntaxFactory.XmlName(
            //                        SyntaxFactory.Identifier("summary"))))
            //            .WithContent(
            //                SyntaxFactory.SingletonList<XmlNodeSyntax>(
            //                    SyntaxFactory.XmlText()
            //                    .WithTextTokens(
            //                        SyntaxFactory.TokenList(
            //                            SyntaxFactory.XmlTextLiteral(
            //                                SyntaxFactory.TriviaList(),
            //                                "test",
            //                                "test",
            //                                SyntaxFactory.TriviaList()))))),
            //            SyntaxFactory.XmlText()
            //            .WithTextTokens(
            //                SyntaxFactory.TokenList(
            //                    SyntaxFactory.XmlTextNewLine(
            //                        SyntaxFactory.TriviaList(),
            //                        "\n",
            //                        "\n",
            //                        SyntaxFactory.TriviaList())))}));


            //TypeDeclarationSyntax newMethodNode = declaration.WithModifiers(
            //    SyntaxFactory.TokenList(
            //        new[]{

            //            SyntaxFactory.Token(
            //                SyntaxFactory.TriviaList(
            //                    SyntaxFactory.Trivia(testDocumentation)), // xmldoc
            //                    SyntaxKind.PublicKeyword, // original 1st token
            //                    SyntaxFactory.TriviaList()),
            //            SyntaxFactory.Token(SyntaxKind.StaticKeyword)}));

            //SyntaxNode syntaxNode = syntaxRoot.ReplaceNode(declaration, newMethodNode);

            //var newDocument = document.WithSyntaxRoot(syntaxNode);



            //SyntaxToken staticKeyWord = SyntaxFactory.Token(SyntaxKind.StaticKeyword).WithTrailingTrivia(SyntaxFactory.Space);
            //var newModifiers = declaration.Modifiers.Add(staticKeyWord);
            //var newMethodDeclaration = declaration.WithModifiers(newModifiers);

            //// Replace the old local declaration with the new local declaration.
            //var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            //var newRoot = oldRoot.ReplaceNode(declaration, newMethodDeclaration);

            //return document.WithSyntaxRoot(newRoot);

            //var documentEditor = await DocumentEditor.CreateAsync(document);
            //documentEditor.ReplaceNode(declaration, newMethodNode);

            //documentEditor.Generator.

            //var newDocument = documentEditor.GetChangedDocument();

            //return newDocument;

            //var newSyntaxRoot  = syntaxRoot.InsertNodesBefore(typeSymbol, testDocumentation);
            //return document.WithSyntaxRoot(newSyntaxRoot);

            //var newDeclaration = declaration.InsertTriviaBefore(declaration.ParentTrivia, new List<SyntaxTrivia> { testDocumentation });



            //// Produce a new solution that has all references to that type renamed, including the declaration.
            //var originalSolution = document.Project.Solution;
            //var optionSet = originalSolution.Workspace.Options;



            ////var newSolution = document.Project.Solution;

            //var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            //// Return the new solution with the now-uppercase type name.
            //return newSolution;
            #endregion
        }

        private async Task<Solution> MakeUppercaseAsync(Document document, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken)
        {
            // Compute new uppercase name.
            var identifierToken = typeDecl.Identifier;
            var newName = identifierToken.Text.ToUpperInvariant();

            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDecl, cancellationToken);

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            // Return the new solution with the now-uppercase type name.
            return newSolution;
        }
    }

    internal class CodeFixResources
    {
        public static string CodeFixTitle = "Generate documentation";
    }
}
