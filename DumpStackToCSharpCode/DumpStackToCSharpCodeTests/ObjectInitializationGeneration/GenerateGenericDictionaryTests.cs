using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;

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
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create();
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