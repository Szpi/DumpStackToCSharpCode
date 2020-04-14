using FluentAssertions;
using NUnit.Framework;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Factory;
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
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(new Dictionary<string, IReadOnlyList<string>>(), false);
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

        [Test]
        public void ShouldGenerate_HashSetOfGuid()
        {
            var stackObject = new ExpressionData("HashSet<Guid>", "Count = 3", "test123", new List<ExpressionData>()
            {
                new ExpressionData("Guid", "{3494167d-f3ba-446e-a0fe-e653888b1a97}", "[0]", new List<ExpressionData>()
                {
                    new ExpressionData("int", "882120317", "_a", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("short", "-3142", "_b", new List<ExpressionData>()
                    {
                    }, "short"),
                    new ExpressionData("short", "17518", "_c", new List<ExpressionData>()
                    {
                    }, "short"),
                    new ExpressionData("byte", "160", "_d", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "254", "_e", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "230", "_f", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "83", "_g", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "136", "_h", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "139", "_i", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "26", "_j", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "151", "_k", new List<ExpressionData>()
                    {
                    }, "byte")
                }, "System.Guid"),
                new ExpressionData("Guid", "{fd86fe4a-cb69-4422-9aed-80310a61a619}", "[1]", new List<ExpressionData>()
                {
                    new ExpressionData("int", "-41484726", "_a", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("short", "-13463", "_b", new List<ExpressionData>()
                    {
                    }, "short"),
                    new ExpressionData("short", "17442", "_c", new List<ExpressionData>()
                    {
                    }, "short"),
                    new ExpressionData("byte", "154", "_d", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "237", "_e", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "128", "_f", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "49", "_g", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "10", "_h", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "97", "_i", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "166", "_j", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "25", "_k", new List<ExpressionData>()
                    {
                    }, "byte")
                }, "System.Guid"),
                new ExpressionData("Guid", "{7a5fa3c7-bb21-4d89-a5b4-cf41e33ef6f3}", "[2]", new List<ExpressionData>()
                {
                    new ExpressionData("int", "2053088199", "_a", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("short", "-17631", "_b", new List<ExpressionData>()
                    {
                    }, "short"),
                    new ExpressionData("short", "19849", "_c", new List<ExpressionData>()
                    {
                    }, "short"),
                    new ExpressionData("byte", "165", "_d", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "180", "_e", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "207", "_f", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "65", "_g", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "227", "_h", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "62", "_i", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "246", "_j", new List<ExpressionData>()
                    {
                    }, "byte"),
                    new ExpressionData("byte", "243", "_k", new List<ExpressionData>()
                    {
                    }, "byte")
                }, "System.Guid")
            }, "System.Collections.Generic.HashSet<System.Guid>");


            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var test123 = new HashSet<Guid>()\n{\r\n    new Guid(\"3494167d-f3ba-446e-a0fe-e653888b1a97\"),\r\n    new Guid(\"fd86fe4a-cb69-4422-9aed-80310a61a619\"),\r\n    new Guid(\"7a5fa3c7-bb21-4d89-a5b4-cf41e33ef6f3\")\r\n};\n");
        }
    }
}
