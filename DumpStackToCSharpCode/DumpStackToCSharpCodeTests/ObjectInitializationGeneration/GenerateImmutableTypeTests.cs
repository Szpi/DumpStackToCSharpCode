using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Factory;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    public class GenerateImmutableTypeTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(new Dictionary<string, IReadOnlyList<string>>(), false);
        }

        [Test]
        public void ShouldGenerate_DateTime()
        {
            var dateTime = new ExpressionData("DateTime", "{10.10.0010 10:10:10}", "Date", new List<ExpressionData>()
            {
                new ExpressionData("int", "10", "Day", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new List<ExpressionData>()
                {
                }, "System.DayOfWeek"),
                new ExpressionData("int", "283", "DayOfYear", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "10", "Hour", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("ulong", "0", "InternalKind", new List<ExpressionData>()
                {
                }, "ulong"),
                new ExpressionData("long", "3083982100000000", "InternalTicks", new List<ExpressionData>()
                {
                }, "long"),
                new ExpressionData("DateTimeKind", "Unspecified", "Kind", new List<ExpressionData>()
                {
                }, "System.DateTimeKind"),
                new ExpressionData("int", "0", "Millisecond", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "10", "Minute", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "10", "Month", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int", "10", "Second", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("long", "3083982100000000", "Ticks", new List<ExpressionData>()
                {
                }, "long"),
                new ExpressionData("int", "10", "Year", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("ulong", "3083982100000000", "_dateData", new List<ExpressionData>()
                {
                }, "ulong")
            }, "System.DateTime");


            var generated = _codeGeneratorManager.GenerateStackDump(dateTime);

            generated.Should().Be("var Date = new DateTime(10, 10, 10, 10, 10, 10, 0, DateTimeKind.Unspecified);\n");
        }
    }
}