using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;

using MattEland.Manticore;
using MattEland.Manticore.Analyzers;

namespace Manticore.Test
{
    /// <summary>
    ///     Unit tests around the <see cref="EndsInTestsAnalyzer"/> and
    ///     <see cref="EndsInTestsCodeFixProvider"/>
    /// </summary>
    [TestClass]
    public class EndsInTestsUnitTests : CodeFixVerifier
    {
        /// <summary>
        ///     No diagnostics expected to show up on an empty string.
        /// </summary>
        [TestMethod]
        public void AnalyzerDoesNotFireOnEmptyString()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        /// <summary>
        ///     Checks that the remove "Tests" from class name analyzer and code fix fire and yield the
        ///     expected results.
        /// </summary>
        [TestMethod]
        public void TestRemoveTestsFromClassNameCodeFix()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class DoSomethingTests
        {   
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = EndsInTestsAnalyzer.DiagnosticId,
                Message = "The class 'DoSomethingTests' ends in 'Tests' but is not in a Test assembly.",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class DoSomething
        {   
        }
    }";
            VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new EndsInTestsCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new EndsInTestsAnalyzer();
        }
    }
}