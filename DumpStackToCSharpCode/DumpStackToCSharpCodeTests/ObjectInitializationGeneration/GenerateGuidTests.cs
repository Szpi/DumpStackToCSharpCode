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
    class GenerateGuidTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(new Dictionary<string, IReadOnlyList<string>>());
        }
        [Test]
        public void ShouldGenerate_Guid()
        {
            var stackObject = new ExpressionData("Guid", "{2a348889-33ee-479e-a986-81f61f62f35f}", "guid", new List<ExpressionData>()
            {
                new ExpressionData("int", "708085897", "_a", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("short", "13294", "_b", new List<ExpressionData>()
                {
                }, "short"),
                new ExpressionData("short", "18334", "_c", new List<ExpressionData>()
                {
                }, "short"),
                new ExpressionData("byte", "169", "_d", new List<ExpressionData>()
                {
                }, "byte"),
                new ExpressionData("byte", "134", "_e", new List<ExpressionData>()
                {
                }, "byte"),
                new ExpressionData("byte", "129", "_f", new List<ExpressionData>()
                {
                }, "byte"),
                new ExpressionData("byte", "246", "_g", new List<ExpressionData>()
                {
                }, "byte"),
                new ExpressionData("byte", "31", "_h", new List<ExpressionData>()
                {
                }, "byte"),
                new ExpressionData("byte", "98", "_i", new List<ExpressionData>()
                {
                }, "byte"),
                new ExpressionData("byte", "243", "_j", new List<ExpressionData>()
                {
                }, "byte"),
                new ExpressionData("byte", "95", "_k", new List<ExpressionData>()
                {
                }, "byte")
            }, "System.Guid");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var guid = new Guid(\"2a348889-33ee-479e-a986-81f61f62f35f\");\n");
        }
    }
}
