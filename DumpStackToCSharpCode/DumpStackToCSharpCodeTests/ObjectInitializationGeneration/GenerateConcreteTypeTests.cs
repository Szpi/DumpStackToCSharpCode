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
    class GenerateConcreteTypeTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(new Dictionary<string, IReadOnlyList<string>>(), true);
        }

        [Test]
        public void ShouldGenerate_ConcurrentBagOfDateTime()
        {
            var stackObject = GetConcurrentBagOfThreeDateTimeDefinition();

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("ConcurrentBag<DateTime> test123 = new ConcurrentBag<DateTime>()\n{\r\n    new DateTime(2018, 4, 9, 0, 9, 25, 874, DateTimeKind.Unspecified),\r\n    new DateTime(2021, 6, 10, 15, 50, 2, 609, DateTimeKind.Unspecified),\r\n    new DateTime(2019, 1, 21, 15, 6, 53, 551, DateTimeKind.Unspecified)\r\n};\n");
        }

        private static ExpressionData GetConcurrentBagOfThreeDateTimeDefinition()
        {
            return new ExpressionData("ConcurrentBag<DateTime>", "Count = 3", "test123", new List<ExpressionData>()
            {
                new ExpressionData("DateTime", "{09.04.2018 00:09:25}", "[0]", new List<ExpressionData>()
                {
                    new ExpressionData("int", "9", "Day", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("DayOfWeek", "Monday", "DayOfWeek", new List<ExpressionData>()
                    {
                    }, "System.DayOfWeek"),
                    new ExpressionData("int", "99", "DayOfYear", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "0", "Hour", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("ulong", "0", "InternalKind", new List<ExpressionData>()
                    {
                    }, "ulong"),
                    new ExpressionData("long", "636588293658743165", "InternalTicks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("DateTimeKind", "Unspecified", "Kind", new List<ExpressionData>()
                    {
                    }, "System.DateTimeKind"),
                    new ExpressionData("int", "874", "Millisecond", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "9", "Minute", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "4", "Month", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "25", "Second", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("long", "636588293658743165", "Ticks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("int", "2018", "Year", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("ulong", "636588293658743165", "_dateData", new List<ExpressionData>()
                    {
                    }, "ulong")
                }, "System.DateTime"),
                new ExpressionData("DateTime", "{10.06.2021 15:50:02}", "[1]", new List<ExpressionData>()
                {
                    new ExpressionData("int", "10", "Day", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("DayOfWeek", "Thursday", "DayOfWeek", new List<ExpressionData>()
                    {
                    }, "System.DayOfWeek"),
                    new ExpressionData("int", "161", "DayOfYear", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "15", "Hour", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("ulong", "0", "InternalKind", new List<ExpressionData>()
                    {
                    }, "ulong"),
                    new ExpressionData("long", "637589370026091452", "InternalTicks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("DateTimeKind", "Unspecified", "Kind", new List<ExpressionData>()
                    {
                    }, "System.DateTimeKind"),
                    new ExpressionData("int", "609", "Millisecond", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "50", "Minute", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "6", "Month", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "2", "Second", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("long", "637589370026091452", "Ticks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("int", "2021", "Year", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("ulong", "637589370026091452", "_dateData", new List<ExpressionData>()
                    {
                    }, "ulong")
                }, "System.DateTime"),
                new ExpressionData("DateTime", "{21.01.2019 15:06:53}", "[2]", new List<ExpressionData>()
                {
                    new ExpressionData("int", "21", "Day", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("DayOfWeek", "Monday", "DayOfWeek", new List<ExpressionData>()
                    {
                    }, "System.DayOfWeek"),
                    new ExpressionData("int", "21", "DayOfYear", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "15", "Hour", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("ulong", "0", "InternalKind", new List<ExpressionData>()
                    {
                    }, "ulong"),
                    new ExpressionData("long", "636836800135514236", "InternalTicks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("DateTimeKind", "Unspecified", "Kind", new List<ExpressionData>()
                    {
                    }, "System.DateTimeKind"),
                    new ExpressionData("int", "551", "Millisecond", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "6", "Minute", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "1", "Month", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "53", "Second", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("long", "636836800135514236", "Ticks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("int", "2019", "Year", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("ulong", "636836800135514236", "_dateData", new List<ExpressionData>()
                    {
                    }, "ulong")
                }, "System.DateTime")
            }, "System.Collections.Concurrent.ConcurrentBag<System.DateTime>");
        }
    }
}
