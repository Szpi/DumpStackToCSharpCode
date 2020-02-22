using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    public class GenerateComplexObjectTypeTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(null);
        }

        [Test]
        public void ShouldGenerate_ComplexObjectAssignment()
        {
            var firstElement = new ExpressionData("string", "test1", "TestString", new ExpressionData[] { }, "string");
            var secondElement = new ExpressionData("int", "10", "TestInt", new ExpressionData[] { }, "int");
            var stackObject = new ExpressionData("Test",
                                                 "Test",
                                                 "testComplexObject",
                                                 new[]
                                                 {
                                                     firstElement,
                                                     secondElement
                                                 },
                                                 "Test");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testComplexObject = new Test()\n{\r\n    TestString = \"test1\",\r\n    TestInt = 10\r\n};\n");
        }


        [Test]
        public void ShouldGenerate_ComplexObjectAssignment_WithStringArray()
        {
            var firstElement = new ExpressionData("string", "test1", "[0]", new ExpressionData[] { }, "string");
            var secondElement = new ExpressionData("string", "test2", "[1]", new ExpressionData[] { }, "string");
            var array = new ExpressionData("string[]",
                                   "{string[2]}",
                                   "testStringArray",
                                   new []
                                   {
                                       firstElement,
                                       secondElement
                                   },
                                   "string[]");

            var stackObject = new ExpressionData("Test", "Test", "testComplexObject", new [] { array }, "Test");
            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testComplexObject = new Test()\n{\r\n    testStringArray = new string[] { \"test1\", \"test2\" }\r\n};\n");
        }

        [Test]
        public void ShouldGenerate_ComplexObjectAssignment_WithInterface()
        {
            var firstElement = new ExpressionData("test", "{ConsoleApp1.test}", "TestTestTest", new ExpressionData[] { }, "ConsoleApp1.Itest {ConsoleApp1.test}");

            var stackObject = new ExpressionData("Test", "Test", "testComplexObject", new [] { firstElement }, "Test");
            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

           generated.Should().Be("var testComplexObject = new Test()\n{\r\n    TestTestTest = new test()\r\n    {\r\n    }\r\n};\n");
        }

        [Test]
        public void ShouldGenerate_ComplexObjectAssignment_WithList()
        {
            var intFirst = new ExpressionData("int", "10", "[0]", new ExpressionData[]
            {
            }, "int");
            var intSecond = new ExpressionData("int", "20", "[1]", new ExpressionData[] { }, "int");

            var listElement = new ExpressionData("List<int>",
                                                 "Count = 2",
                                                 "testListOfInt",
                                                 new[]
                                                 {
                                                     intFirst,
                                                     intSecond
                                                 },
                                                 "System.Collections.Generic.List<int>");
            var stackObject = new ExpressionData("Test", "Test", "testComplexObject", new[] { listElement }, "Test");
            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testComplexObject = new Test()\n{\r\n    testListOfInt = new List<int>() { 10, 20 }\r\n};\n");
        }
        [Test]
        public void ShouldGenerate_ComplexObject()
        {
            var stackObject = new ExpressionData("MainConfiguration", "{Configuration.MainConfiguration}", "test", new List<ExpressionData>()
            {
                new ExpressionData("ComplexType1", "{Configuration.ComplexType1}", "ComplexType1", new List<ExpressionData>()
                {
                    new ExpressionData("bool", "true", "Boolololol", new List<ExpressionData>()
                    {
                    }, "bool"),
                    new ExpressionData("List<int>", "Count = 3", "IntTest", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "191", "[0]", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "140", "[1]", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("int", "146", "[2]", new List<ExpressionData>()
                        {
                        }, "int")
                    }, "System.Collections.Generic.List<int>"),
                    new ExpressionData("List<string>", "Count = 3", "MixStringTest", new List<ExpressionData>()
                    {
                        new ExpressionData("string", "7530fe2e-f008-485c-8482-a9549ad7058a", "[0]", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "2b1c002e-4f59-4082-bdc0-fcde6c5c4855", "[1]", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "069f4a7e-7e78-4484-83c9-feecf6d41aef", "[2]", new List<ExpressionData>()
                        {
                        }, "string")
                    }, "System.Collections.Generic.List<string>"),
                    new ExpressionData("List<string>", "Count = 3", "StringTest", new List<ExpressionData>()
                    {
                        new ExpressionData("string", "274f3147-1c5b-4e1b-99bb-f55846c1186f", "[0]", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "8a35fff2-69ba-4b8c-8136-9ac372681b4b", "[1]", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "3e048a58-e0cf-44dd-b541-a75c97a8cf9a", "[2]", new List<ExpressionData>()
                        {
                        }, "string")
                    }, "System.Collections.Generic.List<string>"),
                    new ExpressionData("float", "112", "test", new List<ExpressionData>()
                    {
                    }, "float")
                }, "Configuration.ComplexType1"),
                new ExpressionData("List<ComplexTypes>", "Count = 3", "ComplexTypes", new List<ExpressionData>()
                {
                    new ExpressionData("ComplexTypes", "{Configuration.ComplexTypes}", "[0]", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "53", "IntTest", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("string", "StringTest592fa936-59c2-4c32-9d44-7c2335e30120", "StringTest", new List<ExpressionData>()
                        {
                        }, "string")
                    }, "Configuration.ComplexTypes"),
                    new ExpressionData("ComplexTypes", "{Configuration.ComplexTypes}", "[1]", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "118", "IntTest", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("string", "StringTest85542230-a956-424a-8715-3ec439e330cc", "StringTest", new List<ExpressionData>()
                        {
                        }, "string")
                    }, "Configuration.ComplexTypes"),
                    new ExpressionData("ComplexTypes", "{Configuration.ComplexTypes}", "[2]", new List<ExpressionData>()
                    {
                        new ExpressionData("int", "138", "IntTest", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("string", "StringTest3dd51148-084f-4035-8996-53b42afc885f", "StringTest", new List<ExpressionData>()
                        {
                        }, "string")
                    }, "Configuration.ComplexTypes")
                }, "System.Collections.Generic.List<Configuration.ComplexTypes>"),
                new ExpressionData("TestConfiguration", "{Configuration.TestConfiguration}", "TestConfiguration", new List<ExpressionData>()
                {
                    new ExpressionData("int", "36", "testoooo", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "170", "testoooo1", new List<ExpressionData>()
                    {
                    }, "int")
                }, "Configuration.TestConfiguration")
            }, "Configuration.MainConfiguration");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var test = new MainConfiguration()\n{\r\n    ComplexType1 = new ComplexType1()\r\n    {\r\n        Boolololol = true,\r\n        IntTest = new List<int>() { 191, 140, 146 },\r\n        MixStringTest = new List<string>() { \"7530fe2e-f008-485c-8482-a9549ad7058a\", \"2b1c002e-4f59-4082-bdc0-fcde6c5c4855\", \"069f4a7e-7e78-4484-83c9-feecf6d41aef\" },\r\n        StringTest = new List<string>() { \"274f3147-1c5b-4e1b-99bb-f55846c1186f\", \"8a35fff2-69ba-4b8c-8136-9ac372681b4b\", \"3e048a58-e0cf-44dd-b541-a75c97a8cf9a\" },\r\n        test = 112F\r\n    },\r\n    ComplexTypes = new List<ComplexTypes>()\r\n    {\r\n        new ComplexTypes()\r\n        {\r\n            IntTest = 53,\r\n            StringTest = \"StringTest592fa936-59c2-4c32-9d44-7c2335e30120\"\r\n        },\r\n        new ComplexTypes()\r\n        {\r\n            IntTest = 118,\r\n            StringTest = \"StringTest85542230-a956-424a-8715-3ec439e330cc\"\r\n        },\r\n        new ComplexTypes()\r\n        {\r\n            IntTest = 138,\r\n            StringTest = \"StringTest3dd51148-084f-4035-8996-53b42afc885f\"\r\n        }\r\n    },\r\n    TestConfiguration = new TestConfiguration()\r\n    {\r\n        testoooo = 36,\r\n        testoooo1 = 170\r\n    }\r\n};\n");
        }
    }
}