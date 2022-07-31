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

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var token = root.FindToken(diagnosticSpan.Start).Parent;

            CodeAction CreateCodeAction(Func<CancellationToken, Task<Document>> documentGenerator)
            {
                return CodeAction.Create(CodeFixTitle,
                    documentGenerator,
                    nameof(CodeFixTitle));
            }

            var codeAction = token switch
            {
                TypeDeclarationSyntax cd => CreateCodeAction(c => AddMissingDocumentation(context.Document, cd, _ => XmlDocumentationGenerator.ForType(cd), c)),
                MethodDeclarationSyntax md => CreateCodeAction(c => AddMissingDocumentation(context.Document, md, _ => XmlDocumentationGenerator.ForMethod(md), c)),
                PropertyDeclarationSyntax pd => CreateCodeAction(c => AddMissingDocumentation(context.Document, pd, _ => XmlDocumentationGenerator.ForProperty(pd), c)),
                _ => throw new NotImplementedException(),
            };

            context.RegisterCodeFix(codeAction, diagnostic);
        }


        private async Task<Document> AddMissingDocumentation(Document document,
            MemberDeclarationSyntax declaration,
            Func<MemberDeclarationSyntax, DocumentationCommentTriviaSyntax> documentationGenerator,
            CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var testDocumentation = documentationGenerator(declaration);

            var documentationTrivia = SyntaxFactory.Trivia(testDocumentation);
            var newDeclaration = declaration.WithLeadingTrivia(documentationTrivia);
            var syntaxNode = syntaxRoot.ReplaceNode(declaration, newDeclaration);
            return document.WithSyntaxRoot(syntaxNode);
        }

        //private async Task<Document> AddMissingDocumentation(Document document,
        //    TypeDeclarationSyntax declaration,
        //    CancellationToken cancellationToken)
        //{
        //    var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        //    var testDocumentation = XmlDocumentationGenerator.ForMember(declaration);

        //    var documentationTrivia = SyntaxFactory.Trivia(testDocumentation);
        //    var newDeclaration = declaration.WithLeadingTrivia(documentationTrivia);
        //    var syntaxNode = syntaxRoot.ReplaceNode(declaration, newDeclaration);
        //    return document.WithSyntaxRoot(syntaxNode);
        //}

        //private async Task<Document> AddMissingDocumentation(Document document,
        //    MethodDeclarationSyntax declaration,
        //    CancellationToken cancellationToken)
        //{
        //    var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        //    var testDocumentation = XmlDocumentationGenerator.ForMember(declaration);

        //    var documentationTrivia = SyntaxFactory.Trivia(testDocumentation);
        //    var newDeclaration = declaration.WithLeadingTrivia(documentationTrivia);
        //    var syntaxNode = syntaxRoot.ReplaceNode(declaration, newDeclaration);
        //    return document.WithSyntaxRoot(syntaxNode);
        //}

        //private async Task<Document> AddMissingDocumentation(Document document,
        //   PropertyDeclarationSyntax declaration,
        //   CancellationToken cancellationToken)
        //{
        //    var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        //    var testDocumentation = XmlDocumentationGenerator.ForMember(declaration);

        //    var documentationTrivia = SyntaxFactory.Trivia(testDocumentation);
        //    var newDeclaration = declaration.WithLeadingTrivia(documentationTrivia);
        //    var syntaxNode = syntaxRoot.ReplaceNode(declaration, newDeclaration);
        //    return document.WithSyntaxRoot(syntaxNode);
        //}
    }
}
