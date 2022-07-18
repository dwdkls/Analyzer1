using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Analyzer1
{
    /// <summary>
    /// super generator
    /// </summary>
    public static class XmlDocumentationGenerator
    {
        public static DocumentationCommentTriviaSyntax ForClassName(string className)
        {
            var summary = SyntaxFactory.XmlSummaryElement(
                SyntaxFactory.XmlNewLine(Environment.NewLine),
                SyntaxFactory.XmlText(className),
                SyntaxFactory.XmlNewLine(Environment.NewLine)
            );

            var doc = SyntaxFactory.DocumentationComment(summary)
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

            return doc;
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
    }
}
