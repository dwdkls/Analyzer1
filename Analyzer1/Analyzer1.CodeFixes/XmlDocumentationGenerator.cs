using System;
using System.Collections.Generic;
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


            XmlElementSyntax returns = BuildReturns(declaration.ReturnType);


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

        private static XmlElementSyntax BuildReturns(TypeSyntax type)
        {
            bool IsValidReturnType(string typeName) => typeName != null && typeName != VoidTypeName;

            bool IsVowel(char c) => c != default && "aeiouAEIOU".IndexOf(c) >= 0;

            string Article(string s) => IsVowel(s.FirstOrDefault()) ? "An" : "A";

            return type switch
            {
                IdentifierNameSyntax returnType
                    when returnType.Identifier.ValueText is string returnTypeName && IsValidReturnType(returnTypeName) =>
                        SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText(
                            $"{Article(returnTypeName)} {returnTypeName} value.")),

                GenericNameSyntax genericReturnType
                    when genericReturnType?.Identifier.ValueText is string genericTypeName =>
                        SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText(
                            $"{Article(genericTypeName)} {BuildGenericTypeName(genericTypeName, genericReturnType)} value.")),

                ArrayTypeSyntax arrayType
                    when arrayType.ElementType.ToFullString() is string arrayTypeName =>
                        SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText(
                            $"An array of {arrayTypeName} value.")),

                _ => null,
            };
        }

        private static string BuildGenericTypeName(string rootName, TypeSyntax type)
        {
            return type switch
            {
                PredefinedTypeSyntax predefined => predefined.Keyword.ValueText,

                IdentifierNameSyntax returnType => returnType.Identifier.ValueText,

                GenericNameSyntax generic =>
                    $"{generic?.Identifier.ValueText} of {string.Join(" and ", generic.TypeArgumentList.Arguments.Select(a => BuildGenericTypeName(rootName, a)))}",

                ArrayTypeSyntax arrayType => $"array of {arrayType.ElementType}",

                _ => string.Empty,
            };
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
