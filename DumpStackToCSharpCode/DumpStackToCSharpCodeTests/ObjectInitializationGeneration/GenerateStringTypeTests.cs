using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    class GenerateStringTypeTests
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
        public void ShouldGenerate_StringWithQuotationMarkAndBackslash()
        {            
            var stackObject = new ExpressionData("string", "\" sdasd \\\\\\\\ \\\"test\\\" asdasd ' asd\"", "testString", new ExpressionData[] { }, "string");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testString = \" sdasd \\\\ \\\"test\\\" asdasd ' asd\";\n");
        }

        [Test]
        public void ShouldGenerate_StringWithQuotationMarkAndTwoBackslash()
        {
            var stackObject = new ExpressionData("string", "\" sdasd \\\\\\\\\\\\\\\\ \\\"test\\\" asdasd ' asd\"", "testString", new ExpressionData[] { }, "string");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testString = \" sdasd \\\\\\\\ \\\"test\\\" asdasd ' asd\";\n");
        }

        [Test]
        public void ShouldGenerate_StringWithQuotationMarkAndTwoBackslashAndSpaceBetween()
        {
            var stackObject = new ExpressionData("string", "\" sdasd \\\\\\\\ \\\\\\\\ \\\\\\\\ \\\"test\\\" asdasd ' asd\"", "testString", new ExpressionData[] { }, "string");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testString = \" sdasd \\\\ \\\\ \\\\ \\\"test\\\" asdasd ' asd\";\n");
        }
    }
}
