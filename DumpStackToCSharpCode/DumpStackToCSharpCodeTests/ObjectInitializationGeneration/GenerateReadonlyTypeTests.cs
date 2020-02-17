using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
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
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(readOnlyTypeConstructorDefinition);
        }

        [Test]
        public void ShouldGenerate_ReadonlyTypeViaConstructor()
        {
            var stackObject = new ExpressionData("ExpressionData", "{ConsoleApp1.Program.ExpressionData}", "mainObject", new List<ExpressionData>()
            {
                new ExpressionData("string", "\\\"testList\\", "Name", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "\\\"List<ExpressionData>\\", "Type", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "\\\"System.Collections.Generic.List<ConsoleApp1.Program.ExpressionData>\\", "TypeWithNamespace", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("List<ExpressionData>", "Count = 1", "UnderlyingExpressionData", new List<ExpressionData>()
                {
                    new ExpressionData("ExpressionData", "{ConsoleApp1.Program.ExpressionData}", "[0]", new List<ExpressionData>()
                    {
                        new ExpressionData("string", "\\\"[0]\\", "Name", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "\\\"ExpressionData\\", "Type", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "\\\"ConsoleApp1.Program.ExpressionData\\", "TypeWithNamespace", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("List<ExpressionData>", "Count = 1", "UnderlyingExpressionData", new List<ExpressionData>()
                        {
                            new ExpressionData("ExpressionData", "{ConsoleApp1.Program.ExpressionData}", "[0]", new List<ExpressionData>()
                            {
                                new ExpressionData("string", "\\\"Name\\", "Name", new List<ExpressionData>()
                                {
                                }, "string"),
                                new ExpressionData("string", "\\\"string\\", "Type", new List<ExpressionData>()
                                {
                                }, "string"),
                                new ExpressionData("string", "\\\"string\\", "TypeWithNamespace", new List<ExpressionData>()
                                {
                                }, "string"),
                                new ExpressionData("IReadOnlyList<ExpressionData>", "null", "UnderlyingExpressionData", new List<ExpressionData>()
                                {
                                }, "System.Collections.Generic.IReadOnlyList<ConsoleApp1.Program.ExpressionData>"),
                                new ExpressionData("string", "\\\"ExpressionData21212\\", "Value", new List<ExpressionData>()
                                {
                                }, "string")
                            }, "ConsoleApp1.Program.ExpressionData")
                        }, "System.Collections.Generic.IReadOnlyList<ConsoleApp1.Program.ExpressionData> {System.Collections.Generic.List<ConsoleApp1.Program.ExpressionData>}"),
                        new ExpressionData("string", "\\\"{ConsoleApp1.Program.ExpressionData}\\", "Value", new List<ExpressionData>()
                        {
                        }, "string")
                    }, "ConsoleApp1.Program.ExpressionData")
                }, "System.Collections.Generic.IReadOnlyList<ConsoleApp1.Program.ExpressionData> {System.Collections.Generic.List<ConsoleApp1.Program.ExpressionData>}"),
                new ExpressionData("string", "\\\"Count = 1\\", "Value", new List<ExpressionData>()
                {
                }, "string")
            }, "ConsoleApp1.Program.ExpressionData");



            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be(@"var mainObject = new ExpressionData(""List<ExpressionData>"", ""Count = 1"", ""testList"", new List<ExpressionData>()
                                {
                                    new ExpressionData(""ExpressionData"", ""{ConsoleApp1.Program.ExpressionData}"", ""[0]"", new List<ExpressionData>()
                                    {
                                        new ExpressionData(""string"", ""ExpressionData21212"", ""Name"", null, ""string"")
                                    }, ""ConsoleApp1.Program.ExpressionData"")
                                }, ""System.Collections.Generic.List<ConsoleApp1.Program.ExpressionData>"");");
        }
    }
}