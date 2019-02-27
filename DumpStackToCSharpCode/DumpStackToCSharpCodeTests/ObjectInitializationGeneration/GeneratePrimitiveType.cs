using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    public class GeneratePrimitiveType
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create();
        }

        [Test]
        public void ShouldGenerate_SimpleIntAssignment()
        {
            var stackObject = new List<ExpressionData>()
            {
                new ExpressionData("int", "10", "testInt", new ExpressionData[] { }, "int")
            };

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testInt = 10;\n");
        }

        [Test]
        public void ShouldGenerate_BuiltInEnumAssignment()
        {
            var stackObject = new List<ExpressionData>()
            {
                new ExpressionData("DateTimeKind", "Utc", "testBuildInEnum", new ExpressionData[] { }, "System.DateTimeKind")
            };

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testBuildInEnum = DateTimeKind.Utc;\n");
        }

        [Test]
        public void ShouldGenerate_SimpleStringAssignment()
        {
            var stackObject = new List<ExpressionData>()
            {
                new ExpressionData("string", "10", "testString", new ExpressionData[] { }, "string")
            };

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testString = \"10\";\n");
        }
    }
}
