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
    class GenerateNullableTimeSpanTypeTests
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
        public void ShouldGenerate_TimeSpan()
        {
            var stackObject = GetTimeSpanDefinition();

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);
            generated.Should().Be("var nullableTimeSpan = new Nullable<TimeSpan>(new TimeSpan(3, 21, 42, 24, 53));\n");
        }

        private ExpressionData GetTimeSpanDefinition()
        {
            return new ExpressionData("TimeSpan?", "{System.TimeSpan}", "nullableTimeSpan", new List<ExpressionData>()
            {
                new ExpressionData("int", "3", "Days", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "21", "Hours", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "53", "Milliseconds", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "42", "Minutes", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "24", "Seconds", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("long", "3373440530000", "Ticks", new List<ExpressionData>()
                {
                }, "long"),
                new ExpressionData("double", "3.90444505787037", "TotalDays", new List<ExpressionData>()
                {
                }, "double"),
                new ExpressionData("double", "93.706681388888882", "TotalHours", new List<ExpressionData>()
                {
                }, "double"),
                new ExpressionData("double", "337344053", "TotalMilliseconds", new List<ExpressionData>()
                {
                }, "double"),
                new ExpressionData("double", "5622.4008833333337", "TotalMinutes", new List<ExpressionData>()
                {
                }, "double"),
                new ExpressionData("double", "337344.05299999996", "TotalSeconds", new List<ExpressionData>()
                {
                }, "double"),
                new ExpressionData("long", "3373440530000", "_ticks", new List<ExpressionData>()
                {
                }, "long")
            }, "System.TimeSpan?");
        }
    }
}
