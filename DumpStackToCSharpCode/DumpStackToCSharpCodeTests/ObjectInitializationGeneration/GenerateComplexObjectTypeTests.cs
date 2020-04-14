using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Factory;

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
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(null, false);
        }

        [Test]
        public void ShouldGenerate_ComplexObjectAssignment()
        {
            var stackObject = new ExpressionData("Test", "{ConsoleApp1.Program.Test}", "test123", new List<ExpressionData>()
            {
                new ExpressionData("List<int>", "Count = 3", "testListOfInt", new List<ExpressionData>()
                {
                    new ExpressionData("int", "84", "[0]", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "193", "[1]", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "167", "[2]", new List<ExpressionData>()
                    {
                    }, "int")
                }, "System.Collections.Generic.List<int>")
            }, "ConsoleApp1.Program.Test");


            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var test123 = new Test()\n{\r\n    testListOfInt = new List<int>() { 84, 193, 167 }\r\n};\n");
        }


        [Test]
        public void ShouldGenerate_ComplexObjectAssignment_WithStringArray()
        {
            var stackObject = new ExpressionData("Test", "{ConsoleApp1.Program.Test}", "test123", new List<ExpressionData>()
            {
                new ExpressionData("string[]", "{string[3]}", "TestStringArray", new List<ExpressionData>()
                {
                    new ExpressionData("string", "26d027f0-238e-4672-90ea-7bb489c35408", "[0]", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("string", "b27bbabc-f00d-4142-b031-5bfb2f94f0aa", "[1]", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("string", "5847d2a8-138a-49a5-973f-7ce86b700c7b", "[2]", new List<ExpressionData>()
                    {
                    }, "string")
                }, "string[]")
            }, "ConsoleApp1.Program.Test");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var test123 = new Test()\n{\r\n    TestStringArray = new string[] { \"26d027f0-238e-4672-90ea-7bb489c35408\", \"b27bbabc-f00d-4142-b031-5bfb2f94f0aa\", \"5847d2a8-138a-49a5-973f-7ce86b700c7b\" }\r\n};\n");
        }

        [Test]
        public void ShouldGenerate_ComplexObjectAssignment_WithInterface()
        {
            var stackObject = new ExpressionData("Test", "{ConsoleApp1.Program.Test}", "testInterface", new List<ExpressionData>()
            {
                new ExpressionData("ITestImplementation", "{ConsoleApp1.Program.ITestImplementation}", "TestInterface", new List<ExpressionData>()
                {
                    new ExpressionData("int", "20", "TestInt", new List<ExpressionData>()
                    {
                    }, "int")
                }, "ConsoleApp1.Program.ITest {ConsoleApp1.Program.ITestImplementation}")
            }, "ConsoleApp1.Program.Test");


            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testInterface = new Test()\n{\r\n    TestInterface = new ITestImplementation()\r\n    {\r\n        TestInt = 20\r\n    }\r\n};\n");
        }

        [Test]
        public void ShouldGenerate_ComplexObjectAssignment_WithList()
        {
            var stackObject = new ExpressionData("List<Test>", "Count = 3", "test123", new List<ExpressionData>()
            {
                new ExpressionData("Test", "{ConsoleApp1.Program.Test}", "[0]", new List<ExpressionData>()
                {
                    new ExpressionData("decimal?", "106", "TestDecimal", new List<ExpressionData>()
                    {
                    }, "decimal?"),
                    new ExpressionData("int", "102", "TestInt", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int?", "28", "TestNullableInt", new List<ExpressionData>()
                    {
                    }, "int?"),
                    new ExpressionData("string", "TestStringb4073650-af31-47a3-a64e-8555da37ff9a", "TestString", new List<ExpressionData>()
                    {
                    }, "string")
                }, "ConsoleApp1.Program.Test"),
                new ExpressionData("Test", "{ConsoleApp1.Program.Test}", "[1]", new List<ExpressionData>()
                {
                    new ExpressionData("decimal?", "145", "TestDecimal", new List<ExpressionData>()
                    {
                    }, "decimal?"),
                    new ExpressionData("int", "50", "TestInt", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int?", "25", "TestNullableInt", new List<ExpressionData>()
                    {
                    }, "int?"),
                    new ExpressionData("string", "TestString76a1d3c9-0bb9-4651-9b2d-68363f96ddda", "TestString", new List<ExpressionData>()
                    {
                    }, "string")
                }, "ConsoleApp1.Program.Test"),
                new ExpressionData("Test", "{ConsoleApp1.Program.Test}", "[2]", new List<ExpressionData>()
                {
                    new ExpressionData("decimal?", "120", "TestDecimal", new List<ExpressionData>()
                    {
                    }, "decimal?"),
                    new ExpressionData("int", "11", "TestInt", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int?", "136", "TestNullableInt", new List<ExpressionData>()
                    {
                    }, "int?"),
                    new ExpressionData("string", "TestString1c8653d0-62d2-4a99-a087-3a21d49764b4", "TestString", new List<ExpressionData>()
                    {
                    }, "string")
                }, "ConsoleApp1.Program.Test")
            }, "System.Collections.Generic.List<ConsoleApp1.Program.Test>");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var test123 = new List<Test>()\n{\r\n    new Test()\r\n    {\r\n        TestDecimal = 106M,\r\n        TestInt = 102,\r\n        TestNullableInt = 28,\r\n        TestString = \"TestStringb4073650-af31-47a3-a64e-8555da37ff9a\"\r\n    },\r\n    new Test()\r\n    {\r\n        TestDecimal = 145M,\r\n        TestInt = 50,\r\n        TestNullableInt = 25,\r\n        TestString = \"TestString76a1d3c9-0bb9-4651-9b2d-68363f96ddda\"\r\n    },\r\n    new Test()\r\n    {\r\n        TestDecimal = 120M,\r\n        TestInt = 11,\r\n        TestNullableInt = 136,\r\n        TestString = \"TestString1c8653d0-62d2-4a99-a087-3a21d49764b4\"\r\n    }\r\n};\n");
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