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

        [Test]
        public void ShouldGenerate_Regex()
        {
            var stackObject = GetRegexDefinition();
            
            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var regex = new Regex(\"<([A-Z][A-Z0-9]*)\\\\b[^>]*>(.*?)</\\\\1>\");\n");
        }

        private ExpressionData GetRegexDefinition()
        {
            return new ExpressionData("Regex", "{<([A-Z][A-Z0-9]*)\\b[^>]*>(.*?)</\\\\1>}", "regex", new List<ExpressionData>()
            {
                new ExpressionData("TimeSpan", "{-00:00:00.0010000}", "MatchTimeout", new List<ExpressionData>()
                {
                    new ExpressionData("int", "0", "Days", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "0", "Hours", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "-1", "Milliseconds", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "0", "Minutes", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "0", "Seconds", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("long", "-10000", "Ticks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("double", "-1.1574074074074074E-08", "TotalDays", new List<ExpressionData>()
                    {
                    }, "double"),
                    new ExpressionData("double", "-2.7777777777777776E-07", "TotalHours", new List<ExpressionData>()
                    {
                    }, "double"),
                    new ExpressionData("double", "-1", "TotalMilliseconds", new List<ExpressionData>()
                    {
                    }, "double"),
                    new ExpressionData("double", "-1.6666666666666667E-05", "TotalMinutes", new List<ExpressionData>()
                    {
                    }, "double"),
                    new ExpressionData("double", "-0.001", "TotalSeconds", new List<ExpressionData>()
                    {
                    }, "double"),
                    new ExpressionData("long", "-10000", "_ticks", new List<ExpressionData>()
                    {
                    }, "long")
                }, "System.TimeSpan"),
                new ExpressionData("RegexOptions", "None", "Options", new List<ExpressionData>()
                {
                }, "System.Text.RegularExpressions.RegexOptions")
            }, "System.Text.RegularExpressions.Regex");

        }
    }
}
