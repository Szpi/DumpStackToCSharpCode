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

            var stackObject = new ExpressionData("List<DateTime>",
                                                 "Count = 1",
                                                 "testListOfDatetime",
                                                 new[]
                                                 {
                                                     dateTimeFirst
                                                 },
                                                 "System.Collections.Generic.List<System.DateTime>");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testListOfDatetime = new List<DateTime>()\n{\r\n    new DateTime(10, 10, 10, 10, 10, 10, 0, DateTimeKind.Unspecified)\r\n};\n");
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