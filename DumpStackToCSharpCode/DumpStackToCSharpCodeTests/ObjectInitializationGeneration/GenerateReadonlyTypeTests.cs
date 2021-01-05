using FluentAssertions;
using NUnit.Framework;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Factory;
using System.Collections.Generic;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    public class GenerateReadonlyType
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            var readOnlyTypeConstructorDefinition = new Dictionary<string, IReadOnlyList<string>>
            {
                ["ExpressionData"] = new List<string> { "type", "value", "name", "underlyingExpressionData", "typeWithNamespace" }

            };
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(readOnlyTypeConstructorDefinition, false);
        }

        [Test]
        public void ShouldGenerate_ReadonlyTypeViaConstructor()
        {
            var stackObject = GetReadonlyTypeDefinition();

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var mainObject = new ExpressionData(\"List<ExpressionData>\",\n\"Count = 1\",\n\"testList\",\nnew List<ExpressionData>()\n{\r\n    new ExpressionData(\"ExpressionData\", \"{ConsoleApp1.Program.ExpressionData}\", \"[0]\", new List<ExpressionData>()\r\n    {\r\n        new ExpressionData(\"string\", \"ExpressionData21212\", \"Name\", null, \"string\")\r\n    }, \"ConsoleApp1.Program.ExpressionData\")\r\n},\n\"System.Collections.Generic.List<ConsoleApp1.Program.ExpressionData>\");\n");
        }

        private static ExpressionData GetReadonlyTypeDefinition()
        {
            return new ExpressionData("ExpressionData", "{ConsoleApp1.Program.ExpressionData}", "mainObject", new List<ExpressionData>()
            {
                new ExpressionData("string", "testList", "Name", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "List<ExpressionData>", "Type", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "System.Collections.Generic.List<ConsoleApp1.Program.ExpressionData>", "TypeWithNamespace", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("List<ExpressionData>", "Count = 1", "UnderlyingExpressionData", new List<ExpressionData>()
                {
                    new ExpressionData("ExpressionData", "{ConsoleApp1.Program.ExpressionData}", "[0]", new List<ExpressionData>()
                    {
                        new ExpressionData("string", "[0]", "Name", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "ExpressionData", "Type", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "ConsoleApp1.Program.ExpressionData", "TypeWithNamespace", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("List<ExpressionData>", "Count = 1", "UnderlyingExpressionData", new List<ExpressionData>()
                        {
                            new ExpressionData("ExpressionData", "{ConsoleApp1.Program.ExpressionData}", "[0]", new List<ExpressionData>()
                            {
                                new ExpressionData("string", "Name", "Name", new List<ExpressionData>()
                                {
                                }, "string"),
                                new ExpressionData("string", "string", "Type", new List<ExpressionData>()
                                {
                                }, "string"),
                                new ExpressionData("string", "string", "TypeWithNamespace", new List<ExpressionData>()
                                {
                                }, "string"),
                                new ExpressionData("IReadOnlyList<ExpressionData>", "null", "UnderlyingExpressionData", new List<ExpressionData>()
                                {
                                }, "System.Collections.Generic.IReadOnlyList<ConsoleApp1.Program.ExpressionData>"),
                                new ExpressionData("string", "ExpressionData21212", "Value", new List<ExpressionData>()
                                {
                                }, "string")
                            }, "ConsoleApp1.Program.ExpressionData")
                        }, "System.Collections.Generic.IReadOnlyList<ConsoleApp1.Program.ExpressionData> {System.Collections.Generic.List<ConsoleApp1.Program.ExpressionData>}"),
                        new ExpressionData("string", "{ConsoleApp1.Program.ExpressionData}", "Value", new List<ExpressionData>()
                        {
                        }, "string")
                    }, "ConsoleApp1.Program.ExpressionData")
                }, "System.Collections.Generic.IReadOnlyList<ConsoleApp1.Program.ExpressionData> {System.Collections.Generic.List<ConsoleApp1.Program.ExpressionData>}"),
                new ExpressionData("string", "Count = 1", "Value", new List<ExpressionData>()
                {
                }, "string")
            }, "ConsoleApp1.Program.ExpressionData");
        }
    }
}