using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzer1
{
    public static class XmlDocumentationGenerator
    {
        public static DocumentationCommentTriviaSyntax ForClass(TypeDeclarationSyntax declaration)
        {
            var summary = SyntaxFactory.XmlSummaryElement(
                SyntaxFactory.XmlNewLine(Environment.NewLine),
                SyntaxFactory.XmlText(declaration.Identifier.Text),
                SyntaxFactory.XmlNewLine(Environment.NewLine)
            );

            return SyntaxFactory.DocumentationComment(summary)
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
        }

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

        public static DocumentationCommentTriviaSyntax ForMethod(MethodDeclarationSyntax declaration)
        {
            var summary = SyntaxFactory.XmlSummaryElement(
                   SyntaxFactory.XmlNewLine(Environment.NewLine),
                   SyntaxFactory.XmlText(declaration.Identifier.Text),
                   SyntaxFactory.XmlNewLine(Environment.NewLine)
            );

            var returnType = ((IdentifierNameSyntax)declaration.ReturnType).Identifier;
            var returnTypeName = returnType.ValueText;


            if (returnTypeName != "void")
            {
                bool IsVovel(char c) => "aeiouAEIOU".IndexOf(c) >= 0;

                var article = IsVovel(returnTypeName[0]) ? "An" : "A";

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
    }
}
