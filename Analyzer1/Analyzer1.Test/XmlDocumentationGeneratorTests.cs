using FluentAssertions;
using Xunit;

namespace Analyzer1.Test
{
    public class XmlDocumentationGeneratorTests
    {
        [Fact]
        public void ForClassName_GenerateMultilineDocumentation()
        {
            string expected = @"/// <summary>
/// MySuperClass
/// </summary>";

            var actual = XmlDocumentationGenerator.ForClassName("MySuperClass").ToFullString();

            actual.Should().Be(expected);
        }
    }
}
