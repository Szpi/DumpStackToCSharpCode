using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using System.Collections.Generic;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    public class GenerateGenericListTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(new Dictionary<string, IReadOnlyList<string>>());
        }

        [Test]
        public void ShouldGenerate_ConcurrentBagOfDateTime()
        {
            var stackObject = GetConcurrentBagOfThreeDateTimeDefinition();

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var test123 = new ConcurrentBag<DateTime>()\n{\r\n    new DateTime(2018, 4, 9, 0, 9, 25, 874, DateTimeKind.Unspecified),\r\n    new DateTime(2021, 6, 10, 15, 50, 2, 609, DateTimeKind.Unspecified),\r\n    new DateTime(2019, 1, 21, 15, 6, 53, 551, DateTimeKind.Unspecified)\r\n};\n");
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

        [Test]
        public void ShouldGenerate_ListOfComplexType()
        {
            var stackObject = new ExpressionData("Test", "{ConsoleApp1.Program.Test}", "test123", new List<ExpressionData>()
            {
                new ExpressionData("List<int>", "Count = 3", "IntList", new List<ExpressionData>()
                {
                    new ExpressionData("int", "221", "[0]", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "195", "[1]", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "10", "[2]", new List<ExpressionData>()
                    {
                    }, "int")
                }, "System.Collections.Generic.List<int>"),
                new ExpressionData("List<string>", "Count = 3", "IntString", new List<ExpressionData>()
                {
                    new ExpressionData("string", "ef311e42-8e09-4f69-8332-880e87b02add", "[0]", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("string", "3d5f03ec-556e-4ca5-9bf5-2162a392b2be", "[1]", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("string", "4df373a1-5785-4787-94cb-0b5bddadac0b", "[2]", new List<ExpressionData>()
                    {
                    }, "string")
                }, "System.Collections.Generic.List<string>")
            }, "ConsoleApp1.Program.Test");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);
            
            generated.Should().Be("var test123 = new Test()\n{\r\n    IntList = new List<int>() { 221, 195, 10 },\r\n    IntString = new List<string>() { \"ef311e42-8e09-4f69-8332-880e87b02add\", \"3d5f03ec-556e-4ca5-9bf5-2162a392b2be\", \"4df373a1-5785-4787-94cb-0b5bddadac0b\" }\r\n};\n");
        }
    }
}