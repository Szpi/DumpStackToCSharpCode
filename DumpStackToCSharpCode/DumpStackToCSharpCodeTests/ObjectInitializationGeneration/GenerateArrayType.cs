using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using System.Collections.Generic;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    public class GenerateArrayType
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create();
        }

        [Test]
        public void ShouldGenerate_ArrayAssignment()
        {
            var firstElement = new ExpressionData("string", "test1", "[0]", new ExpressionData[] { }, "string");
            var secondElement = new ExpressionData("string", "test2", "[1]", new ExpressionData[] { }, "string");
            var stackObject = new List<ExpressionData>()
            {
                new ExpressionData("string[]", "{string[2]}", "testStringArray", new ExpressionData[] { firstElement, secondElement }, "string[]")
            };

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testStringArray = new string[]{\"test1\",\"test2\"};\n");
        }

        [Test]
        public void ShouldGenerate_IntAssignment()
        {
            var firstElement = new ExpressionData("int", "11", "[0]", new ExpressionData[] { }, "int");
            var secondElement = new ExpressionData("int", "10", "[1]", new ExpressionData[] { }, "int");
            var thirdElement = new ExpressionData("int", "13", "[2]", new ExpressionData[] { }, "int");
            var stackObject = new List<ExpressionData>()
            {
                new ExpressionData("int[]", "{int[2]}", "testIntArray", new [] { firstElement, secondElement, thirdElement }, "int[]")
            };

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testIntArray = new string[]{11, 10, 13};\n");
        }
    }
}