using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace Analyzer1.Test
{
    public class XmlDocumentationGeneratorTests
    {
        [Fact]
        public void ForClass_GenerateMultilineDocumentation()
        {
            var typeDeclaration = SyntaxFactory.TypeDeclaration(SyntaxKind.ClassDeclaration, "MySuperClass");

            string expected = @"/// <summary>
/// MySuperClass
/// </summary>
";

            var actual = XmlDocumentationGenerator.ForClass(typeDeclaration).ToFullString();

            actual.Should().Be(expected);
        }

//        [Fact]
//        public void ForMethod_GenerateMultilineDocumentation()
//        {
//            string expected = @"/// <summary>
///// MySuperMethod
///// </summary>
//";

//            var actual = XmlDocumentationGenerator.ForClass("MySuperClass").ToFullString();

//            actual.Should().Be(expected);
//        }
    }
}

/*
/// <param name="str">Describe parameter.</param>
/// <param name="num">Describe parameter.</param>
/// <param name="ptr">Describe parameter.</param>
/// <returns>Describe return value.</returns>
/// */