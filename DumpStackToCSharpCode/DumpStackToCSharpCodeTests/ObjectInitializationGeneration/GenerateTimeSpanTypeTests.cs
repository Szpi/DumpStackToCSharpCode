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
    class GenerateTimeSpanTypeTests
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
        public void ShouldGenerate_TimeSpan()
        {
            var stackObject = GetTimeSpanDefinition();

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var timeSpan = new TimeSpan(0, 10, 10, 10, 0);\n");
        }

        private ExpressionData GetTimeSpanDefinition()
        {
            return new ExpressionData("TimeSpan", "{System.TimeSpan}", "timeSpan", new List<ExpressionData>()
            {
                new ExpressionData("int", "0", "Days", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "10", "Hours", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "0", "Milliseconds", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "10", "Minutes", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "10", "Seconds", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("long", "366100000000", "Ticks", new List<ExpressionData>()
                {
                }, "long"),
                new ExpressionData("double", "0.42372685185185183", "TotalDays", new List<ExpressionData>()
                {
                }, "double"),
                new ExpressionData("double", "10.169444444444444", "TotalHours", new List<ExpressionData>()
                {
                }, "double"),
                new ExpressionData("double", "36610000", "TotalMilliseconds", new List<ExpressionData>()
                {
                }, "double"),
                new ExpressionData("double", "610.16666666666663", "TotalMinutes", new List<ExpressionData>()
                {
                }, "double"),
                new ExpressionData("double", "36610", "TotalSeconds", new List<ExpressionData>()
                {
                }, "double"),
                new ExpressionData("long", "366100000000", "_ticks", new List<ExpressionData>()
                {
                }, "long")
            }, "System.TimeSpan");
        }
    }
}
