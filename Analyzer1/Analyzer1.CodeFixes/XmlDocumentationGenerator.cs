using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzer1
{
    public static class XmlDocumentationGenerator
    {
        public static DocumentationCommentTriviaSyntax ForType(TypeDeclarationSyntax declaration)
        {
            var summary = SyntaxFactory.XmlSummaryElement(
                SyntaxFactory.XmlNewLine(Environment.NewLine),
                SyntaxFactory.XmlText(declaration.Identifier.Text),
                SyntaxFactory.XmlNewLine(Environment.NewLine)
            );

            return SyntaxFactory.DocumentationComment(summary)
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
        }

        #region SO code
        /* var testDocumentation = SyntaxFactory.DocumentationCommentTrivia(
                SyntaxKind.SingleLineDocumentationCommentTrivia, SyntaxFactory.List(new XmlNodeSyntax[] { SyntaxFactory.XmlText()
                .WithTextTokens(SyntaxFactory.TokenList(
                    SyntaxFactory.XmlTextLiteral(
                        SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")), " ", " ", SyntaxFactory.TriviaList()))),
                    SyntaxFactory.XmlElement(
                            SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName(SyntaxFactory.Identifier("summary"))),
                            SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName(SyntaxFactory.Identifier("summary"))))
                        .WithContent(SyntaxFactory.SingletonList<XmlNodeSyntax>(
                            SyntaxFactory.XmlText().WithTextTokens(SyntaxFactory.TokenList(
                                SyntaxFactory.XmlTextLiteral(SyntaxFactory.TriviaList(), className, className, SyntaxFactory.TriviaList()))))),
                    SyntaxFactory.XmlText()
                        .WithTextTokens(SyntaxFactory.TokenList(
                            SyntaxFactory.XmlTextNewLine(SyntaxFactory.TriviaList(), Environment.NewLine, Environment.NewLine, SyntaxFactory.TriviaList()))) }));

            return testDocumentation;
        */
        #endregion

        public static DocumentationCommentTriviaSyntax ForMethod(MethodDeclarationSyntax declaration)
        {
            var summary = SyntaxFactory.XmlSummaryElement(
                   SyntaxFactory.XmlNewLine(Environment.NewLine),
                   SyntaxFactory.XmlText(declaration.Identifier.Text),
                   SyntaxFactory.XmlNewLine(Environment.NewLine)
            );

            var returnType = declaration.ReturnType as PredefinedTypeSyntax;
            var returnTypeName = returnType.Keyword.ValueText;

            if (returnTypeName != "void")
            {
                bool IsVowel(char c) => "aeiouAEIOU".IndexOf(c) >= 0;

                var article = IsVowel(returnTypeName[0]) ? "An" : "A";

                var returns = SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText($"{article} {returnTypeName} value."));

                return SyntaxFactory.DocumentationComment(
                    summary,
                    SyntaxFactory.XmlNewLine(Environment.NewLine),
                    returns)
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
            }
            else
            {
                return SyntaxFactory.DocumentationComment(summary)
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
            }
        }

        public static DocumentationCommentTriviaSyntax ForProperty(PropertyDeclarationSyntax declaration)
        {
            var summary = SyntaxFactory.XmlSummaryElement(
               SyntaxFactory.XmlNewLine(Environment.NewLine),
               SyntaxFactory.XmlText(declaration.Identifier.Text),
               SyntaxFactory.XmlNewLine(Environment.NewLine)
           );

            return SyntaxFactory.DocumentationComment(summary)
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
        }
    }
}
