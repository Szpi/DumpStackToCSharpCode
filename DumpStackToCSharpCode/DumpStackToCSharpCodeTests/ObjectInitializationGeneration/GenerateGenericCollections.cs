using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using System.Collections.Generic;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    public class GenerateGenericCollections
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create();
        }

        [Test]
        public void ShouldGenerate_ListOfDateTime()
        {
            var dateTimeFirst = new ExpressionData("DateTime", "[0]", "Date", new ExpressionData[]
            {
                new ExpressionData("int", "10", "Day", new ExpressionData[] { }, "int"),
                new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new ExpressionData[] { }, "System.DayOfWeek"),
                new ExpressionData("int", "283", "DayOfYear", new ExpressionData[] { }, "int"),
                new ExpressionData("int", "10", "Hour", new ExpressionData[] { }, "int"),
                new ExpressionData("ulong", "0", "InternalKind", new ExpressionData[] { }, "ulong"),
                new ExpressionData("long", "3083982100000000", "InternalTicks", new ExpressionData[] { }, "long"),
                new ExpressionData("DateTimeKind", "Unspecified", "Kind", new ExpressionData[] { }, "System.DateTimeKind"),
                new ExpressionData("int", "0", "Millisecond", new ExpressionData[] { }, "int"),
                new ExpressionData("int", "10", "Minute", new ExpressionData[] { }, "int"),
                new ExpressionData("int", "10", "Month", new ExpressionData[] { }, "int"),
                new ExpressionData("int", "10", "Second", new ExpressionData[] { }, "int"),
                new ExpressionData("int", "10", "Year", new ExpressionData[] { }, "int"),
                new ExpressionData("long", "3083982100000000", "Ticks", new ExpressionData[] { }, "long"),
            }, "System.DateTime");

            var stackObject = new List<ExpressionData>()
            {
                new ExpressionData("List<DateTime>", "Count = 1", "testListOfDatetime", new[] {dateTimeFirst }, "System.Collections.Generic.List<System.DateTime>")
            };

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testListOfDatetime = new List<DateTime>()\n{\r\n    new DateTime(10, 10, 10, 10, 10, 10, 0, DateTimeKind.Unspecified)\r\n};\n");
        }
    }
}