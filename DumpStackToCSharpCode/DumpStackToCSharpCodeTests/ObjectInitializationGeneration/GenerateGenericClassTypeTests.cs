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
    class GenerateGenericClassTypeTests
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
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(readOnlyTypeConstructorDefinition, false);
        }

        [Test]
        public void ShouldGenerate_GenericClass()
        {
            var stackObject = GetGenericClassDefinition();

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var genericClass = new GenericClass<string>()\n{\r\n    TestType = \"test\"\r\n};\n");
        }

        private ExpressionData GetGenericClassDefinition()
        {
            return new ExpressionData("GenericClass<string>", "{ConsoleApp1.Program.GenericClass<string>}", "genericClass", new List<ExpressionData>()
            {
                new ExpressionData("string", "test", "TestType", new List<ExpressionData>()
                {
                }, "string")
            }, "ConsoleApp1.Program.GenericClass<string>");
        }
    }
}
