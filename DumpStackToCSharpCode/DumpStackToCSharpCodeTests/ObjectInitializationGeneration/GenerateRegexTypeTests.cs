using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    class GenerateRegexTypeTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            var readOnlyTypeConstructorDefinition = new Dictionary<string, IReadOnlyList<string>>
            {
                ["ExpressionData"] = new List<string> { "type", "value", "name", "underlyingExpressionData", "typeWithNamespace" }

            };
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(readOnlyTypeConstructorDefinition);
        }

        [Ignore("temporary")]
        [Test]
        public void ShouldGenerate_TimeSpan()
        {
            var stackObject = GetRegexDefinition();

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var timeSpan = new TimeSpan(0, 10, 10, 10, 0);\n");
        }

        private ExpressionData GetRegexDefinition()
        {
            return null;
        }
    }
}
