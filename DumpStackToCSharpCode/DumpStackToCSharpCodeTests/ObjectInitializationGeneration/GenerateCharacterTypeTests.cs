using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using System.Collections.Generic;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    class GenerateCharacterTypeTests
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

        [Test]
        public void ShouldGenerate_CharacterWithApostrophe()
        {
            var stackObject = new ExpressionData("char", "'\\''", "testString", new ExpressionData[] { }, "char");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testString = \'\\\'\';\n");
        }

        [TestCase("a")]
        [TestCase("x")]
        [TestCase("Q")]
        [TestCase(" ")]
        [TestCase("^")]
        [TestCase("\\")]
        public void ShouldGenerate_SimpleCharacter(string charString)
        {
            var stackObject = new ExpressionData("char", $"'{charString}'", "testString", new ExpressionData[] { }, "char");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be($"var testString = \'{charString}\';\n");
        }
    }
}
