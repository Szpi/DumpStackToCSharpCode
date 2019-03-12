using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration.Type
{
    [TestFixture]
    public class ConcreteTypeAnalyzerTests
    {
        private ConcreteTypeAnalyzer _concreteTypeAnalyzer;

        [SetUp]
        public void SetUp()
        {
            _concreteTypeAnalyzer = new ConcreteTypeAnalyzer();
        }

        [TestCase("string, System.Collections.Generic.Dictionary<int, string>", "string,Dictionary<int, string>")]
        [TestCase("test.namespace.test, System.Collections.Generic.Dictionary<int, test.a.b.c.d.test>", "test,Dictionary<int,test>")]
        [TestCase("System.Collections.Generic.List<int>", "List<int>")]
        [TestCase("System.Collections.Generic.List<test.namespace.test>", "List<test>")]
        [TestCase("System.Collections.Generic.List<System.Collections.Generic.List<test.namespace.test>>", "List<List<test>>")]
        public void GetForGenericType_RemoveNamespace_FromType(string input , string expected)
        {
            var result = _concreteTypeAnalyzer.GetTypeWithoutNamespace(input);

            result.Should().Be(expected);
        }
    }
}