using FluentAssertions;
using NUnit.Framework;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    class GenerateComplexObjectWithInheritanceTypeTests
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
        public void ShouldGenerate_ComplexTypeWithInheritance()
        {
            var stackObject = GetComplexTypeWithInheritanceDefinition();

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var inheritacne = new MainClass()\n{\r\n    TestBase = 10,\r\n    TestBaseString = \" test\",\r\n    TestMain = 10\r\n};\n");
        }

        private ExpressionData GetComplexTypeWithInheritanceDefinition()
        {
            return new ExpressionData("MainClass", "{ConsoleApp1.Program.MainClass}", "inheritacne", new List<ExpressionData>()
            {
                new ExpressionData("int", "10", "TestBase", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("string", " test", "TestBaseString", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("int", "10", "TestMain", new List<ExpressionData>()
                {
                }, "int")
            }, "ConsoleApp1.Program.MainClass");
        }
    }
}
