using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Analyzer1.Test.CSharpCodeFixVerifier<
    Analyzer1.Analyzer1Analyzer,
    Analyzer1.Analyzer1CodeFixProvider>;

namespace Analyzer1.Test
{
    [TestClass]
    public class Analyzer1UnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            var test = @"
public class {|#0:TypeName|}
{
    public static void Test()
    {
    }
}";

            var fixtest = @"
public static class TypeName
{
    public static void Test()
    {
    }
}";

            var expected = VerifyCS.Diagnostic("Analyzer1").WithLocation(0).WithArguments("TypeName");
            //await VerifyCS.VerifyAnalyzerAsync(test, expected);
            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }
    }
}
