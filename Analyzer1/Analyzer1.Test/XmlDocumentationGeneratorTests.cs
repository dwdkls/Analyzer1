using System;
using FluentAssertions;
using Microsoft.CodeAnalysis;
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
            var typeDeclaration = SyntaxFactory.TypeDeclaration(SyntaxKind.ClassDeclaration, "MyTestClass");

            string expected = @"/// <summary>
/// MyTestClass
/// </summary>
";

            var actual = XmlDocumentationGenerator.ForType(typeDeclaration).ToFullString();

            actual.Should().Be(expected);
        }


        [Fact]
        public void ForVoidMethodWithoutParams_GenerateMultilineDocumentationWithoutParamsAndReturns()
        {
            TypeSyntax ts = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("void"));

            var methodDeclaration = SyntaxFactory.MethodDeclaration(ts, "MyTestMethod");

            string expected = @"/// <summary>
/// MyTestMethod
/// </summary>
";

            var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ForIntMethodWithoutParams_GenerateMultilineDocumentationWithoutParamsAndReturns()
        {
            var returnType = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("int"));
            var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

            string expected = @"/// <summary>
/// MyTestMethod
/// </summary>
/// <returns>An int value.</returns>
";

            var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ForListOfIntMethodWithoutParams_GenerateMultilineDocumentationWithoutParamsAndReturns()
        {
            var returnType = SyntaxFactory.GenericName(
                SyntaxFactory.Identifier("List"),
                SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.ParseTypeName("int") })));

            SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("int"));
            var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");


            string expected = @"/// <summary>
/// MyTestMethod
/// </summary>
/// <returns>A List of int value.</returns>
";

            var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ForStringMethodWithoutParams_GenerateMultilineDocumentationWithoutParamsAndReturns()
        {
            var returnType = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("string"));
            var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

            string expected = @"/// <summary>
/// MyTestMethod
/// </summary>
/// <returns>A string value.</returns>
";

            var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ForMyClassMethodWithoutParams_GenerateMultilineDocumentationWithoutParamsAndReturns()
        {
            var returnType = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("MyClass"));
            var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

            string expected = @"/// <summary>
/// MyTestMethod
/// </summary>
/// <returns>A MyClass value.</returns>
";

            var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ForVoidMethodWithParams_GenerateMultilineDocumentationWithoutParamsAndWithoutReturns()
        {
            TypeSyntax ts = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("void"));

            var methodDeclaration = SyntaxFactory.MethodDeclaration(ts, "MyTestMethod")
                .WithParameterList(SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Parameter(SyntaxFactory.Identifier("testName")) })));

            string expected = @"/// <summary>
/// MyTestMethod
/// </summary>
/// <param name=""testName"">testName</param>
/// <param name=""testAge"">testAge</param>
/// <param name=""testCustomClass"">testCustomClass</param>
";

            var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

            actual.Should().Be(expected);
        }
    }
}

/*
/// <param name="str">Describe parameter.</param>
/// <param name="num">Describe parameter.</param>
/// <param name="ptr">Describe parameter.</param>
/// <returns>Describe return value.</returns>
/// */