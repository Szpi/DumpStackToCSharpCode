using DumpStackToCSharpCode.Command.Util;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DumpStackToCSharpCodeTests.Command.Util
{
    [TestFixture]
    class ArgumentsListPurifierTests
    {
        private ArgumentsListPurifier _argumentsListPurifier;

        [SetUp]
        public void SetUp()
        {
            _argumentsListPurifier = new ArgumentsListPurifier();
        }

        [Test]
        public void Purify_ShouldTrim_Input()
        {
            var testString = "TestString";
            var chosenLocals = new List<string>()
            {
                testString,
                testString + " ",
                " " +testString + " ",
            };

            var result = _argumentsListPurifier.Purify(chosenLocals);

            foreach (var item in result)
            {
                item.Should().Be(testString);
            }
        }

        [Test]
        public void Purify_ShouldTrimSplitConstructorArgumentsInputLike()
        {
            var input = new List<string>()
            {
                " string type",
                "string value ",
                "string name ",
                "IReadOnlyList<ExpressionData> underlyingExpressionData ",
                "string typeWithNamespace   "
            };

            var expected = new List<string>()
            {
                "type",
                "value",
                "name",
                "underlyingExpressionData",
                "typeWithNamespace",
            };

            var result = _argumentsListPurifier.Purify(input);

            result.Should().BeEquivalentTo(expected);
        }
    }
}
