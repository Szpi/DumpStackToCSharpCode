using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Expression;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.Initialization
{
    public class RegexInitializationManager
    {
        private readonly ComplexTypeInitializationGenerator _complexTypeInitializationGenerator;
        private readonly PrimitiveExpressionGenerator _primitiveExpressionGenerator;

        public RegexInitializationManager(ComplexTypeInitializationGenerator complexTypeInitializationGenerator,
            PrimitiveExpressionGenerator primitiveExpressionGenerator)
        {
            _complexTypeInitializationGenerator = complexTypeInitializationGenerator;
            _primitiveExpressionGenerator = primitiveExpressionGenerator;
        }

        public (SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, List<ExpressionSyntax> argumentSyntax) Generate(ExpressionData expressionData)
        {
            var newExpressionData = new ExpressionData(expressionData.Type, expressionData.Value, expressionData.Name, null, expressionData.TypeWithNamespace);
            var regexPattern = _primitiveExpressionGenerator.Generate(TypeCode.String, expressionData.Value.Trim('{', '}').Replace("\\\\", "\\"));

            var generated = _complexTypeInitializationGenerator.Generate(newExpressionData, new SeparatedSyntaxList<ExpressionSyntax>());
            return (new SeparatedSyntaxList<ExpressionSyntax>().Add(generated), new List<ExpressionSyntax> { regexPattern });
        }
    }
}
