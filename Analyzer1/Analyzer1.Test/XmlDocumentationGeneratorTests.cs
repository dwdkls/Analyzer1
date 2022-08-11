using System;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace Analyzer1.Test;

public class XmlDocumentationGeneratorTests
{
    [Fact]
    public void Class_GenerateMultilineDocumentation()
    {
        var typeDeclaration = SyntaxFactory.TypeDeclaration(SyntaxKind.ClassDeclaration, "MyTestClassWithLongName");

        string expected = @"/// <summary>
/// My Test Class With Long Name
/// </summary>
";

        var actual = XmlDocumentationGenerator.ForType(typeDeclaration).ToFullString();

        actual.Should().Be(expected);
    }


    [Fact]
    public void VoidMethodWithoutParams_GenerateMultilineDocumentationWithoutParamsAndReturns()
    {
        TypeSyntax ts = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("void"));

        var methodDeclaration = SyntaxFactory.MethodDeclaration(ts, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void IntMethodWithoutParams_GenerateMultilineDocumentationWithoutParamsAndReturns()
    {
        var returnType = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("int"));
        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>An int.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void StringMethodWithoutParams_GenerateMultilineDocumentationWithoutParamsAndReturns()
    {
        var returnType = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("string"));
        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>A string.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void MyClassMethodWithoutParams_GenerateMultilineDocumentationWithoutParamsAndReturns()
    {
        var returnType = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("MyClass"));
        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>A MyClass.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void ListOfIntMethodWithoutParams_GenerateProperGenericTypeReturns()
    {
        var returnType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("List"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.ParseTypeName("int") })));

        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>A List of int.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void DictionaryOfStringAndIntMethodWithoutParams_GenerateProperGenericTypeReturns()
    {
        var returnType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("Dictionary"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(
                new[] { SyntaxFactory.ParseTypeName("string"), SyntaxFactory.ParseTypeName("int") })));

        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>A Dictionary of string and int.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void DictionaryOfStringAndArrayOfIntMethodWithoutParams_GenerateProperGenericTypeReturns()
    {
        TypeSyntax arrayOfInt = SyntaxFactory.ArrayType(SyntaxFactory.ParseTypeName("int"));
        var returnType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("Dictionary"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(
                new[] { SyntaxFactory.ParseTypeName("string"), arrayOfInt })));

        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>A Dictionary of string and array of int.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void DictionaryOfStringAndListOfIntMethodWithoutParams_GenerateProperGenericTypeReturns()
    {
        TypeSyntax genericListOfInt = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("List"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.ParseTypeName("int") })));
        var returnType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("Dictionary"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(
                new[] { SyntaxFactory.ParseTypeName("string"), genericListOfInt })));

        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>A Dictionary of string and List of int.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void ArrayOfIntMethodWithoutParams_GenerateProperGenericTypeReturns()
    {
        var returnType = SyntaxFactory.ArrayType(SyntaxFactory.ParseTypeName("int"));

        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>An array of int.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void ListOfListOfIntMethodWithoutParams_GenerateProperGenericTypeReturns()
    {
        TypeSyntax genericListOfInt = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("List"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.ParseTypeName("int") })));
        TypeSyntax genericListOfListOfInt = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("List"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { genericListOfInt }))
        );
        var returnType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("List"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { genericListOfListOfInt }))
        );

        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>A List of List of List of int.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void ListOfArrayOfIntMethodWithoutParams_GenerateProperGenericTypeReturns()
    {
        TypeSyntax arrayOfInt = SyntaxFactory.ArrayType(SyntaxFactory.ParseTypeName("int"));
        var returnType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("List"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { arrayOfInt }))
        );

        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>A List of array of int.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void ArrayOfListOfIntMethodWithoutParams_GenerateProperGenericTypeReturns()
    {
        TypeSyntax genericListOfInt = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("List"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.ParseTypeName("int") })));
        var returnType = SyntaxFactory.ArrayType(genericListOfInt);

        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>An array of List of int.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void ListOfDictionaryOfStringAndIntMethodWithoutParams_GenerateProperGenericTypeReturn()
    {
        TypeSyntax dictionaryType = SyntaxFactory.GenericName(
           SyntaxFactory.Identifier("Dictionary"),
           SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(
               new[] { SyntaxFactory.ParseTypeName("string"), SyntaxFactory.ParseTypeName("int") })));
        var returnType = SyntaxFactory.GenericName(
            SyntaxFactory.Identifier("List"),
            SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { dictionaryType })));

        var methodDeclaration = SyntaxFactory.MethodDeclaration(returnType, "MyTestMethod");

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <returns>A List of Dictionary of string and int.</returns>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }


    [Fact]
    public void VoidMethodWithParams_GenerateDocumentationWithoutParamsAndWithoutReturns()
    {
        TypeSyntax ts = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("void"));

        var methodDeclaration = SyntaxFactory.MethodDeclaration(ts, "MyTestMethod")
            .WithParameterList(SyntaxFactory.ParameterList(
                SyntaxFactory.SeparatedList(new[] { 
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("testString")),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("testInt")),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("testCustomClass")),
                })));

        string expected = @"/// <summary>
/// My Test Method
/// </summary>
/// <param name=""testString"">A Test String</param>
/// <param name=""testInt"">A Test Int</param>
/// <param name=""testCustomClass"">A Test Custom Class</param>
";

        var actual = XmlDocumentationGenerator.ForMethod(methodDeclaration).ToFullString();

        actual.Should().Be(expected);
    }
}

/*
/// <param name="str">Describe parameter.</param>
/// <param name="num">Describe parameter.</param>
/// <param name="ptr">Describe parameter.</param>
/// <returns>Describe return.</returns>
/// */