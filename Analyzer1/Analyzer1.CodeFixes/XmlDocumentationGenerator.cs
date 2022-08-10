using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Analyzer1;

public static class XmlDocumentationGenerator
{
    private static readonly string VoidTypeName = typeof(void).Name.ToLowerInvariant();

    private static XmlTextSyntax NewLine => SyntaxFactory.XmlNewLine(Environment.NewLine);

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

        var returns = BuildReturns(declaration.ReturnType);

        var documentation = returns != null
            ? SyntaxFactory.DocumentationComment(summary, NewLine, returns)
            : SyntaxFactory.DocumentationComment(summary);

        return documentation.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
    }

    private static XmlElementSyntax BuildReturns(TypeSyntax type)
    {
        bool IsValidReturnType(string typeName) => typeName != null && typeName != VoidTypeName;

        bool IsVowel(char c) => c != default && "aeiouAEIOU".IndexOf(c) >= 0;

        string Article(string s) => IsVowel(s.FirstOrDefault()) ? "An" : "A";

        if (!IsValidReturnType(type.ToString()))
        {
            return null;
        }

        var typeAsString = GetTypeName(type);

        return SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText($"{Article(typeAsString)} {typeAsString} value."));
    }

    private static string GetTypeName(TypeSyntax type)
    {
        return type switch
        {
            PredefinedTypeSyntax predefined => predefined.Keyword.ValueText,
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            GenericNameSyntax generic =>
                $"{generic?.Identifier.ValueText} of {string.Join(" and ", generic.TypeArgumentList.Arguments.Select(GetTypeName))}",
            ArrayTypeSyntax arrayType => $"array of {GetTypeName(arrayType.ElementType)}",
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
