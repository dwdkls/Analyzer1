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
            SyntaxTrivia sol = SyntaxFactory.DocumentationCommentExterior("///");

            XmlTextSyntax startOfLine = SyntaxFactory.XmlText("/// ");
            XmlElementStartTagSyntax startOfSummary = SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName("summary"));
            XmlElementEndTagSyntax endOfSummary = SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName("summary"));
            XmlTextSyntax summaryContent = SyntaxFactory.XmlText(className);


            XmlTextSyntax cc = SyntaxFactory.XmlNewLine(className);

            var scc = cc.ToFullString();

            XmlElementSyntax yy = SyntaxFactory.XmlSummaryElement(summaryContent);

            var syy = yy.ToFullString();

            SyntaxToken uu = SyntaxFactory.XmlTextNewLine(className);

            var suu = uu.ToFullString();

            //var x2 = SyntaxFactory.XmlElement("summary",
            //    SyntaxFactory.SingletonList<XmlNodeSyntax>(SyntaxFactory.XmlText(className)));

            //SyntaxNode node;
            //SyntaxToken token;


            XmlTextSyntax newLine = SyntaxFactory.XmlText(Environment.NewLine);

            //var allNodes = newLine.ChildNodes();
            //var allTokens = newLine.ChildTokens();

            //SyntaxFactory.XmlTextLiteral(

            //var ugagawe = SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///"), SyntaxFactory.XmlTextNewLine();

            //SyntaxFactory.XmlSummaryElement()


            var testDocumentation = SyntaxFactory.DocumentationComment(
                SyntaxFactory.XmlSummaryElement(
                    SyntaxFactory.XmlNewLine(Environment.NewLine),
                    SyntaxFactory.XmlText(className),
                    SyntaxFactory.XmlNewLine(Environment.NewLine)
                )).WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);


            //var testDocumentation = SyntaxFactory.DocumentationComment(
            //        SyntaxFactory.XmlElement("summary", SyntaxFactory.List(new List<XmlNodeSyntax>(new[] {
            //                    SyntaxFactory.XmlText().AddTextTokens(
            //                        SyntaxFactory.XmlTextNewLine(Environment.NewLine, true))
            //                            .AddTextTokens(SyntaxFactory.XmlTextNewLine($"{summaryContent}{Environment.NewLine}", true))
            //                        })
            //            )
            //        )
            //    );


            //DocumentationCommentTriviaSyntax testDocumentation = SyntaxFactory.DocumentationCommentTrivia(
            //    SyntaxKind.MultiLineDocumentationCommentTrivia,
            //    SyntaxFactory.List(new XmlNodeSyntax[] {
            //        //startOfLine,
            //        //yy
            //        startOfLine, 
            //        //startOfSummary,
            //        newLine,
            //        startOfLine, summaryContent, newLine,
            //        startOfLine, 
            //        //endOfSummary, 
            //    })
            //    //newLine.ChildTokens().First()
            ////SyntaxFactory.XmlTextNewLine("", false)
            //);

            //var x = testDocumentation.EndOfComment.ToFullString();

            return testDocumentation;
        }

        /* XmlTextSyntax x1 = SyntaxFactory.XmlText()
                .WithTextTokens(SyntaxFactory.TokenList(
                    SyntaxFactory.XmlTextLiteral(
                        SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")), " ", " ", SyntaxFactory.TriviaList())));

            XmlElementSyntax x2 = SyntaxFactory.XmlElement(
                            SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName(SyntaxFactory.Identifier("summary"))),
                            SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName(SyntaxFactory.Identifier("summary"))))
                        .WithContent(SyntaxFactory.SingletonList<XmlNodeSyntax>(
                            SyntaxFactory.XmlText().WithTextTokens(SyntaxFactory.TokenList(
                                SyntaxFactory.XmlTextLiteral(SyntaxFactory.TriviaList(), "test", "tost", SyntaxFactory.TriviaList())))));

            XmlTextSyntax x3 = SyntaxFactory.XmlText()
                        .WithTextTokens(SyntaxFactory.TokenList(
                            SyntaxFactory.XmlTextNewLine(SyntaxFactory.TriviaList(), Environment.NewLine, Environment.NewLine, SyntaxFactory.TriviaList())));

            DocumentationCommentTriviaSyntax testDocumentation = SyntaxFactory.DocumentationCommentTrivia(
                SyntaxKind.SingleLineDocumentationCommentTrivia, SyntaxFactory.List(new XmlNodeSyntax[] { x1, x2, x3 }));
        */
    }
}
