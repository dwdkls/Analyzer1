using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzer1
{
    public static class XmlDocumentationGenerator
    {
        private static readonly string VoidTypeName = typeof(void).Name.ToLowerInvariant();

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

            //var returnType = declaration.ReturnType as TypeSyntax;
            //var returnTypeName = returnType.Keyword.ValueText;

            // ((Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax)declaration.ReturnType).Identifier


            bool IsValidReturnType(string typeName) => typeName != null && typeName != VoidTypeName;

            bool IsVowel(char c) => c != default && "aeiouAEIOU".IndexOf(c) >= 0;

            string Article(string s) => IsVowel(s.FirstOrDefault()) ? "An" : "A";

            XmlElementSyntax returns = declaration.ReturnType switch
            {
                IdentifierNameSyntax returnType
                    when returnType.Identifier.ValueText is string returnTypeName && IsValidReturnType(returnTypeName) =>
                        SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText(
                            $"{Article(returnTypeName)} {returnTypeName} value.")),

                GenericNameSyntax genericReturnType
                    when genericReturnType?.Identifier.ValueText is string genericTypeName =>
                        SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText(
                            $"{Article(genericTypeName)} {genericTypeName} of {string.Join("and ", genericReturnType.TypeArgumentList.Arguments)} value.")),

                _ => null,
            };


            //if (declaration.ReturnType is IdentifierNameSyntax returnType)
            //{
            //    var returnTypeName = returnType.Identifier.ValueText;

            //    if (IsValidReturnType(returnTypeName))
            //    {
            //        var article = IsVowel(returnTypeName[0]) ? "An" : "A";

            //        var returns = SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText($"{article} {returnTypeName} value."));

            //        return SyntaxFactory.DocumentationComment(
            //            summary,
            //            SyntaxFactory.XmlNewLine(Environment.NewLine),
            //            returns)
            //            .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
            //    }
            //}
            //else if (declaration.ReturnType is GenericNameSyntax genericReturnType)
            //{
            //    var genericTypeName = genericReturnType?.Identifier.ValueText;
            //    var argumentTypeNames = string.Join("and ", genericReturnType.TypeArgumentList.Arguments);

            //    var article = IsVowel(genericTypeName[0]) ? "An" : "A";

            //    var returns = SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText($"{article} {genericTypeName} of {argumentTypeNames} value."));

            //    return SyntaxFactory.DocumentationComment(
            //        summary,
            //        SyntaxFactory.XmlNewLine(Environment.NewLine),
            //        returns)
            //        .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
            //}


            if (returns != null)
            {
                return SyntaxFactory.DocumentationComment(
                        summary,
                        SyntaxFactory.XmlNewLine(Environment.NewLine),
                        returns)
                        .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
            }

            return SyntaxFactory.DocumentationComment(summary)
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
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
