using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Analyzer1
{
    public static class XmlDocumentationGenerator
    {
        public static DocumentationCommentTriviaSyntax ForClassName(string className)
        {
            XmlTextSyntax x1 = SyntaxFactory.XmlText("/// ");
            //.WithTextTokens(
            //     SyntaxFactory.TokenList(SyntaxFactory.XmlTextLiteral("/// ")));
            //SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")), " ", " ", SyntaxFactory.TriviaList())));

            XmlElementSyntax x2 = SyntaxFactory.XmlElement("summary",
                SyntaxFactory.SingletonList<XmlNodeSyntax>(SyntaxFactory.XmlText(className)));
            //SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName(SyntaxFactory.Identifier("summary"))),
            //SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName(SyntaxFactory.Identifier("summary"))))
            //.WithContent(SyntaxFactory.SingletonList<XmlNodeSyntax>(
            //    SyntaxFactory.XmlText().WithTextTokens(
            //        SyntaxFactory.TokenList(
            //        SyntaxFactory.XmlTextLiteral(SyntaxFactory.TriviaList(), className, className, SyntaxFactory.TriviaList())))));

            XmlTextSyntax x3 = SyntaxFactory.XmlText(Environment.NewLine);
                        //.WithTextTokens(SyntaxFactory.TokenList(
                        //    SyntaxFactory.XmlTextNewLine(SyntaxFactory.TriviaList(), Environment.NewLine, Environment.NewLine, SyntaxFactory.TriviaList())));

            DocumentationCommentTriviaSyntax testDocumentation = SyntaxFactory.DocumentationCommentTrivia(
                SyntaxKind.SingleLineDocumentationCommentTrivia, SyntaxFactory.List(new XmlNodeSyntax[] { x1, x2, x3 }));

            return testDocumentation;
        }
    }
}
