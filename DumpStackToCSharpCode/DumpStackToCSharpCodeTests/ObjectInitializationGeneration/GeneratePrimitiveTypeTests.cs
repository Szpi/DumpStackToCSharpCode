using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    public class GeneratePrimitiveTypeTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(null, false);
        }

        [Test]
        public void ShouldGenerate_SimpleIntAssignment()
        {
            var stackObject = new ExpressionData("int", "10", "testInt", new ExpressionData[] { }, "int");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testInt = 10;\n");
        }

        [Test]
        public void ShouldGenerate_SimpleBoolAssignment()
        {
            var stackObject = new ExpressionData("bool", "true", "testBool", new ExpressionData[] { }, "bool");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testBool = true;\n");
        }

        [Test]
        public void ShouldGenerate_BuiltInEnumAssignment()
        {
            var stackObject = new ExpressionData("DateTimeKind",
                                                 "Utc",
                                                 "testBuildInEnum",
                                                 new ExpressionData[]
                                                 {
                                                 },
                                                 "System.DateTimeKind");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testBuildInEnum = DateTimeKind.Utc;\n");
        }

        [Test]
        public void ShouldGenerate_SimpleStringAssignment()
        {
            var stackObject = new ExpressionData("string", "10", "testString", new ExpressionData[] { }, "string");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testString = \"10\";\n");
        }
    }
}
