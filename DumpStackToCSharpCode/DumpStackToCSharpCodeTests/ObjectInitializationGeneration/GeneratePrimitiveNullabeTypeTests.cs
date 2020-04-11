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
    class test
    {
        public int? dsa { get; set; }
        public test(int? test)
        {
            dsa = test;
        }
    }
    [TestFixture]
    class GeneratePrimitiveNullabeTypeTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(null, false);
            var a = new test(10);
        }

        [Test]
        public void ShouldGenerate_NullabeIntAssignment()
        {
            var stackObject = new ExpressionData("int?", "10", "testNullable", new List<ExpressionData>()
            {
            }, "int?");

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var testNullable = new Nullable<int>(10);\n");
        }

        [Test]
        public void ShouldGenerate_NullabeBoolAssignment()
        {
            var stackObject = new ExpressionData("bool?", "true", "nullablebool", new List<ExpressionData>()
            {
            }, "bool?");


            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var nullablebool = new Nullable<bool>(true);\n");
        }
    }
}
