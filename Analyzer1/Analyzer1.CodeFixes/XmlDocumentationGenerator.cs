using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzer1;

public static class XmlDocumentationGenerator
{
    private static readonly string VoidTypeName = typeof(void).Name.ToLowerInvariant();

    public static DocumentationCommentTriviaSyntax ForType(TypeDeclarationSyntax declaration)
    {
        var summaryText = ToSentence(declaration.Identifier.Text);

        var summary = SyntaxFactory.XmlSummaryElement(
            SyntaxFactory.XmlNewLine(Environment.NewLine),
            SyntaxFactory.XmlText(summaryText),
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
        var xmlNodes = new List<XmlNodeSyntax>();

        var summary = SyntaxFactory.XmlSummaryElement(
            SyntaxFactory.XmlNewLine(Environment.NewLine),
            SyntaxFactory.XmlText(ToSentence(declaration.Identifier.Text)),
            SyntaxFactory.XmlNewLine(Environment.NewLine));

        xmlNodes.Add(summary);

        foreach (var parameter in declaration.ParameterList.ChildNodes())
        {
            xmlNodes.Add(SyntaxFactory.XmlNewLine(Environment.NewLine));
            xmlNodes.Add(BuildParameter(parameter));
        }

        if (declaration.ReturnType.ToString() != VoidTypeName)
        {
            xmlNodes.Add(SyntaxFactory.XmlNewLine(Environment.NewLine));
            xmlNodes.Add(BuildReturns(declaration.ReturnType));
        }

        return SyntaxFactory.DocumentationComment(xmlNodes.ToArray())
            .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
    }

    private static XmlElementSyntax BuildParameter(SyntaxNode item)
    {
        var paramName = item.ToFullString();
        var paramDescription = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ToSentence(paramName));
        var paramDescXml = SyntaxFactory.XmlText($"{Article(paramDescription)} {paramDescription}");

        return SyntaxFactory.XmlParamElement(paramName, paramDescXml);
    }

    private static XmlElementSyntax BuildReturns(TypeSyntax type)
    {
        string BuildTypeName(TypeSyntax type)
        {
            return type switch
            {
                PredefinedTypeSyntax predefined => predefined.Keyword.ValueText,
                IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
                GenericNameSyntax generic =>
                    $"{generic?.Identifier.ValueText} of {string.Join(" and ", generic.TypeArgumentList.Arguments.Select(BuildTypeName))}",
                ArrayTypeSyntax arrayType => $"array of {BuildTypeName(arrayType.ElementType)}",
                _ => string.Empty,
            };
        }

        var typeDescription = BuildTypeName(type);
        return SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText($"{Article(typeDescription)} {typeDescription}."));
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

    private static string Article(string s)
    {
        bool IsVowel(char c) => c != default && "aeiouAEIOU".IndexOf(c) >= 0;

        return IsVowel(s.FirstOrDefault()) ? "An" : "A";
    }

    private static string ToSentence(string s)
    {
        return Regex.Replace(s, "(?!^)([A-Z])", " $1");
    }
}
