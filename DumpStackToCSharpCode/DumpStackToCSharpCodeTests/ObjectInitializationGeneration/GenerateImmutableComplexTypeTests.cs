using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using System.Collections.Generic;

namespace DumpStackToCSharpCodeTests
{
    [TestFixture]
    class GenerateImmutableComplexTypeTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(new Dictionary<string, IReadOnlyList<string>>
            {
                ["ReadOnlyClass"] = new List<string>() { "testString", "dateTime", "timeSpan", "dateTimeOffset", "testInt", "testNullableInt", "testDecimal", "intList", "intStringList" }
            }, false);
        }

        [Test]
        public void ShouldGenerate_NullabeIntAssignment()
        {
            var stackObject = GetImmutableComplexType();

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var readonlyClass = new ReadOnlyClass(\"testStringad691a26-4398-4ca6-be89-a84ff8f1d90b\",\nnew Nullable<DateTime>(new DateTime(2021, 11, 7, 3, 9, 45, 771, DateTimeKind.Unspecified)),\nnew Nullable<TimeSpan>(new TimeSpan(10, 0, 0, 0, 0)),\nnew Nullable<DateTimeOffset>(new DateTimeOffset(2018, 9, 2, 14, 5, 36, 423, new TimeSpan(0, 2, 0, 0, 0))),\n84,\nnew Nullable<int>(153),\nnew Nullable<decimal>(144M),\nnew List<int>()\n{\r\n    66,\r\n    232,\r\n    161\r\n},\nnew List<string>()\n{\r\n    \"5fe2da12-02a4-43aa-b35b-05915a60dad4\",\r\n    \"979d153d-cf1b-4ac7-b486-ca1f64977a2b\",\r\n    \"252ae04a-db0a-406a-bd86-b98fb6d22537\"\r\n});\n");
        }

        private ExpressionData GetImmutableComplexType()
        {
            return new ExpressionData("ReadOnlyClass", "{ConsoleApp1.Program.ReadOnlyClass}", "readonlyClass", new List<ExpressionData>()
            {
                new ExpressionData("DateTime?", "{07.11.2021 03:09:45}", "DateTime", new List<ExpressionData>()
                {
                    new ExpressionData("DateTime", "{07.11.2021 00:00:00}", "Date", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "7", "Day", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new List<ExpressionData>()
                        {
                        }, "System.DayOfWeek"),
                        new ExpressionData("int", "311", "DayOfYear", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "0", "Hour", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "0", "InternalKind", new List<ExpressionData>()
                        {
                        }, "ulong"),
                        new ExpressionData("long", "637718400000000000", "InternalTicks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("DateTimeKind", "Unspecified", "Kind", new List<ExpressionData>()
                        {
                        }, "System.DateTimeKind"),
                        new ExpressionData("int", "0", "Millisecond", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "0", "Minute", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "11", "Month", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "0", "Second", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("long", "637718400000000000", "Ticks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("int", "2021", "Year", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "637718400000000000", "_dateData", new List<ExpressionData>()
                        {
                        }, "ulong")
                    }, "System.DateTime"),
                    new ExpressionData("int", "7", "Day", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new List<ExpressionData>()
                    {
                    }, "System.DayOfWeek"),
                    new ExpressionData("int", "311", "DayOfYear", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "3", "Hour", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("ulong", "0", "InternalKind", new List<ExpressionData>()
                    {
                    }, "ulong"),
                    new ExpressionData("long", "637718513857711295", "InternalTicks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("DateTimeKind", "Unspecified", "Kind", new List<ExpressionData>()
                    {
                    }, "System.DateTimeKind"),
                    new ExpressionData("int", "771", "Millisecond", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "9", "Minute", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "11", "Month", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "45", "Second", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("long", "637718513857711295", "Ticks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("TimeSpan", "{03:09:45.7711295}", "TimeOfDay", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "0", "Days", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "3", "Hours", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "771", "Milliseconds", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "9", "Minutes", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "45", "Seconds", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("long", "113857711295", "Ticks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("double", "0.13177975844328704", "TotalDays", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "3.1627142026388886", "TotalHours", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "11385771.1295", "TotalMilliseconds", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "189.76285215833335", "TotalMinutes", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "11385.771129499999", "TotalSeconds", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("long", "113857711295", "_ticks", new List<ExpressionData>()
                        {
                        }, "long")
                    }, "System.TimeSpan"),
                    new ExpressionData("int", "2021", "Year", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("ulong", "637718513857711295", "_dateData", new List<ExpressionData>()
                    {
                    }, "ulong")
                }, "System.DateTime?"),
                new ExpressionData("DateTimeOffset?", "{02.09.2018 14:05:36 +02:00}", "DateTimeOffset", new List<ExpressionData>()
                {
                    new ExpressionData("DateTime", "{02.09.2018 14:05:36}", "ClockDateTime", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "2", "Day", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new List<ExpressionData>()
                        {
                        }, "System.DayOfWeek"),
                        new ExpressionData("int", "245", "DayOfYear", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "14", "Hour", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "0", "InternalKind", new List<ExpressionData>()
                        {
                        }, "ulong"),
                        new ExpressionData("long", "636714939364233524", "InternalTicks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("DateTimeKind", "Unspecified", "Kind", new List<ExpressionData>()
                        {
                        }, "System.DateTimeKind"),
                        new ExpressionData("int", "423", "Millisecond", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "5", "Minute", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "9", "Month", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "36", "Second", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("long", "636714939364233524", "Ticks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("int", "2018", "Year", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "636714939364233524", "_dateData", new List<ExpressionData>()
                        {
                        }, "ulong")
                    }, "System.DateTime"),
                    new ExpressionData("DateTime", "{02.09.2018 00:00:00}", "Date", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "2", "Day", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new List<ExpressionData>()
                        {
                        }, "System.DayOfWeek"),
                        new ExpressionData("int", "245", "DayOfYear", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "0", "Hour", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "0", "InternalKind", new List<ExpressionData>()
                        {
                        }, "ulong"),
                        new ExpressionData("long", "636714432000000000", "InternalTicks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("DateTimeKind", "Unspecified", "Kind", new List<ExpressionData>()
                        {
                        }, "System.DateTimeKind"),
                        new ExpressionData("int", "0", "Millisecond", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "0", "Minute", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "9", "Month", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "0", "Second", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("long", "636714432000000000", "Ticks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("int", "2018", "Year", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "636714432000000000", "_dateData", new List<ExpressionData>()
                        {
                        }, "ulong")
                    }, "System.DateTime"),
                    new ExpressionData("DateTime", "{02.09.2018 14:05:36}", "DateTime", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "2", "Day", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new List<ExpressionData>()
                        {
                        }, "System.DayOfWeek"),
                        new ExpressionData("int", "245", "DayOfYear", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "14", "Hour", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "0", "InternalKind", new List<ExpressionData>()
                        {
                        }, "ulong"),
                        new ExpressionData("long", "636714939364233524", "InternalTicks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("DateTimeKind", "Unspecified", "Kind", new List<ExpressionData>()
                        {
                        }, "System.DateTimeKind"),
                        new ExpressionData("int", "423", "Millisecond", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "5", "Minute", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "9", "Month", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "36", "Second", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("long", "636714939364233524", "Ticks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("int", "2018", "Year", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "636714939364233524", "_dateData", new List<ExpressionData>()
                        {
                        }, "ulong")
                    }, "System.DateTime"),
                    new ExpressionData("int", "2", "Day", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new List<ExpressionData>()
                    {
                    }, "System.DayOfWeek"),
                    new ExpressionData("int", "245", "DayOfYear", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "14", "Hour", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("DateTime", "{02.09.2018 14:05:36}", "LocalDateTime", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "2", "Day", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new List<ExpressionData>()
                        {
                        }, "System.DayOfWeek"),
                        new ExpressionData("int", "245", "DayOfYear", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "14", "Hour", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "9223372036854775808", "InternalKind", new List<ExpressionData>()
                        {
                        }, "ulong"),
                        new ExpressionData("long", "636714939364233524", "InternalTicks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("DateTimeKind", "Local", "Kind", new List<ExpressionData>()
                        {
                        }, "System.DateTimeKind"),
                        new ExpressionData("int", "423", "Millisecond", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "5", "Minute", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "9", "Month", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "36", "Second", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("long", "636714939364233524", "Ticks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("int", "2018", "Year", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "9860086976219009332", "_dateData", new List<ExpressionData>()
                        {
                        }, "ulong")
                    }, "System.DateTime"),
                    new ExpressionData("int", "423", "Millisecond", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "5", "Minute", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "9", "Month", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("TimeSpan", "{02:00:00}", "Offset", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "0", "Days", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "2", "Hours", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "0", "Milliseconds", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "0", "Minutes", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "0", "Seconds", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("long", "72000000000", "Ticks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("double", "0.083333333333333329", "TotalDays", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "2", "TotalHours", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "7200000", "TotalMilliseconds", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "120", "TotalMinutes", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "7200", "TotalSeconds", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("long", "72000000000", "_ticks", new List<ExpressionData>()
                        {
                        }, "long")
                    }, "System.TimeSpan"),
                    new ExpressionData("int", "36", "Second", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("long", "636714939364233524", "Ticks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("TimeSpan", "{14:05:36.4233524}", "TimeOfDay", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "0", "Days", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "14", "Hours", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "423", "Milliseconds", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "5", "Minutes", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "36", "Seconds", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("long", "507364233524", "Ticks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("double", "0.58722712213425921", "TotalDays", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "14.093450931222222", "TotalHours", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "50736423.352400005", "TotalMilliseconds", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "845.60705587333337", "TotalMinutes", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("double", "50736.4233524", "TotalSeconds", new List<ExpressionData>()
                        {
                        }, "double"),
                        new ExpressionData("long", "507364233524", "_ticks", new List<ExpressionData>()
                        {
                        }, "long")
                    }, "System.TimeSpan"),
                    new ExpressionData("DateTime", "{02.09.2018 12:05:36}", "UtcDateTime", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "2", "Day", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new List<ExpressionData>()
                        {
                        }, "System.DayOfWeek"),
                        new ExpressionData("int", "245", "DayOfYear", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "12", "Hour", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "4611686018427387904", "InternalKind", new List<ExpressionData>()
                        {
                        }, "ulong"),
                        new ExpressionData("long", "636714867364233524", "InternalTicks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("DateTimeKind", "Utc", "Kind", new List<ExpressionData>()
                        {
                        }, "System.DateTimeKind"),
                        new ExpressionData("int", "423", "Millisecond", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "5", "Minute", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "9", "Month", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "36", "Second", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("long", "636714867364233524", "Ticks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("int", "2018", "Year", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "5248400885791621428", "_dateData", new List<ExpressionData>()
                        {
                        }, "ulong")
                    }, "System.DateTime"),
                    new ExpressionData("long", "636714867364233524", "UtcTicks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("int", "2018", "Year", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("DateTime", "{02.09.2018 12:05:36}", "_dateTime", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "2", "Day", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("DayOfWeek", "Sunday", "DayOfWeek", new List<ExpressionData>()
                        {
                        }, "System.DayOfWeek"),
                        new ExpressionData("int", "245", "DayOfYear", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "12", "Hour", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "0", "InternalKind", new List<ExpressionData>()
                        {
                        }, "ulong"),
                        new ExpressionData("long", "636714867364233524", "InternalTicks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("DateTimeKind", "Unspecified", "Kind", new List<ExpressionData>()
                        {
                        }, "System.DateTimeKind"),
                        new ExpressionData("int", "423", "Millisecond", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "5", "Minute", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "9", "Month", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "36", "Second", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("long", "636714867364233524", "Ticks", new List<ExpressionData>()
                        {
                        }, "long"),
                        new ExpressionData("int", "2018", "Year", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("ulong", "636714867364233524", "_dateData", new List<ExpressionData>()
                        {
                        }, "ulong")
                    }, "System.DateTime"),
                    new ExpressionData("short", "120", "_offsetMinutes", new List<ExpressionData>()
                    {
                    }, "short")
                }, "System.DateTimeOffset?"),
                new ExpressionData("List<int>", "Count = 3", "IntList", new List<ExpressionData>()
                {
                    new ExpressionData("int", "66", "[0]", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "232", "[1]", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "161", "[2]", new List<ExpressionData>()
                    {
                    }, "int")
                }, "System.Collections.Generic.List<int>"),
                new ExpressionData("List<string>", "Count = 3", "IntStringList", new List<ExpressionData>()
                {
                    new ExpressionData("string", "5fe2da12-02a4-43aa-b35b-05915a60dad4", "[0]", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("string", "979d153d-cf1b-4ac7-b486-ca1f64977a2b", "[1]", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("string", "252ae04a-db0a-406a-bd86-b98fb6d22537", "[2]", new List<ExpressionData>()
                    {
                    }, "string")
                }, "System.Collections.Generic.List<string>"),
                new ExpressionData("decimal?", "144", "TestDecimal", new List<ExpressionData>()
                {
                }, "decimal?"),
                new ExpressionData("int", "84", "TestInt", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("int?", "153", "TestNullableInt", new List<ExpressionData>()
                {
                }, "int?"),
                new ExpressionData("string", "testStringad691a26-4398-4ca6-be89-a84ff8f1d90b", "TestString", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("TimeSpan?", "{00:00:00.0000244}", "TimeSpan", new List<ExpressionData>()
                {
                    new ExpressionData("int", "10", "Days", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "0", "Hours", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "0", "Milliseconds", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "0", "Minutes", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "0", "Seconds", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("long", "244", "Ticks", new List<ExpressionData>()
                    {
                    }, "long"),
                    new ExpressionData("double", "2.8240740740740741E-10", "TotalDays", new List<ExpressionData>()
                    {
                    }, "double"),
                    new ExpressionData("double", "6.7777777777777778E-09", "TotalHours", new List<ExpressionData>()
                    {
                    }, "double"),
                    new ExpressionData("double", "0.0244", "TotalMilliseconds", new List<ExpressionData>()
                    {
                    }, "double"),
                    new ExpressionData("double", "4.0666666666666666E-07", "TotalMinutes", new List<ExpressionData>()
                    {
                    }, "double"),
                    new ExpressionData("double", "2.44E-05", "TotalSeconds", new List<ExpressionData>()
                    {
                    }, "double"),
                    new ExpressionData("long", "244", "_ticks", new List<ExpressionData>()
                    {
                    }, "long")
                }, "System.TimeSpan?")
            }, "ConsoleApp1.Program.ReadOnlyClass");
        }
    }
}
