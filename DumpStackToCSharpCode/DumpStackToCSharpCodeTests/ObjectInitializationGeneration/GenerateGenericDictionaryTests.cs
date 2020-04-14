using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Factory;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    public class GenerateGenericDictionaryTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(null, false);
        }

        [Test]
        public void ShouldGenerate_Dictionary()
        {
            var dictionaryObject = new ExpressionData("Dictionary<int, string>",
                                                      "Count = 3",
                                                      "testDictionary",
                                                      new ExpressionData[]
                                                      {
                                                          GenerateDictionaryElement("0", "test0", 0),
                                                          GenerateDictionaryElement("1", "test1", 1),
                                                          GenerateDictionaryElement("2", "test2", 2)
                                                      },
                                                      "System.Collections.Generic.Dictionary<int, string>");

            var generated = _codeGeneratorManager.GenerateStackDump(dictionaryObject);

            generated.Should().Be("var testDictionary = new Dictionary<int, string>()\n{\r\n    [0] = \"test0\",\r\n    [1] = \"test1\",\r\n    [2] = \"test2\"\r\n};\n");
        }

        [Test]
        public void ShouldGenerate_DictionaryInt()
        {
            var stackObject = new ExpressionData("Dictionary<int, int>", "Count = 3", "dictionary", new List<ExpressionData>()
            {
                new ExpressionData("KeyValuePair<int, int>", "{[0, 0]}", "[0]", new List<ExpressionData>()
                {
                    new ExpressionData("int", "0", "Key", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "0", "Value", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "0", "key", new List<ExpressionData>()
                    {
                    }, "int")
                }, "System.Collections.Generic.KeyValuePair<int, int>"),
                new ExpressionData("KeyValuePair<int, int>", "{[1, 1]}", "[1]", new List<ExpressionData>()
                {
                    new ExpressionData("int", "1", "Key", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "1", "Value", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "1", "key", new List<ExpressionData>()
                    {
                    }, "int")
                }, "System.Collections.Generic.KeyValuePair<int, int>"),
                new ExpressionData("KeyValuePair<int, int>", "{[2, 2]}", "[2]", new List<ExpressionData>()
                {
                    new ExpressionData("int", "2", "Key", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "2", "Value", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("int", "2", "key", new List<ExpressionData>()
                    {
                    }, "int")
                }, "System.Collections.Generic.KeyValuePair<int, int>")
            }, "System.Collections.Generic.Dictionary<int, int>");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);
            generated.Should().Be("var dictionary = new Dictionary<int, int>()\n{\r\n    [0] = 0,\r\n    [1] = 1,\r\n    [2] = 2\r\n};\n");
        }

        private static ExpressionData GenerateDictionaryElement(string keyValue, string valueValue, int index)
        {
            var key = new ExpressionData("int",
                                         keyValue,
                                         "Key",
                                         new List<ExpressionData>(),
                                         "int");

            var value = new ExpressionData("string",
                                           valueValue,
                                           "Value",
                                           new List<ExpressionData>(),
                                           "string");

            var keyDuplicated = new ExpressionData("int",
                                                   keyValue,
                                                   "key",
                                                   new List<ExpressionData>(),
                                                   "int");

            var firstObject = new ExpressionData("KeyValuePair<int, string>",
                                                 $"{{[{keyValue}, {valueValue}]}}",
                                                 $"[{index}]",
                                                 new[]
                                                 {
                                                     key,
                                                     value,
                                                     keyDuplicated
                                                 },
                                                 "System.Collections.Generic.KeyValuePair<int, string>");
            return firstObject;
        }
    }
}