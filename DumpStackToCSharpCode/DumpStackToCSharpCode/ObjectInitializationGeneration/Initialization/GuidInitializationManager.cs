using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Expression;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System.Collections.Generic;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.Initialization
{
    public class GuidInitializationManager
    {
        private readonly ComplexTypeInitializationGenerator _complexTypeInitializationGenerator;
        private readonly PrimitiveExpressionGenerator _primitiveExpressionGenerator;

        public GuidInitializationManager(ComplexTypeInitializationGenerator complexTypeInitializationGenerator,
            PrimitiveExpressionGenerator primitiveExpressionGenerator)
        {
            _complexTypeInitializationGenerator = complexTypeInitializationGenerator;
            _primitiveExpressionGenerator = primitiveExpressionGenerator;
        }
        public (SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, List<ExpressionSyntax> argumentSyntax) Generate(ExpressionData expressionData)
        {
            if (expressionData.UnderlyingExpressionData.Count != 11)
            {
                return (new SeparatedSyntaxList<ExpressionSyntax>(), null);
            }

            var guid = new System.Guid(
                uint.Parse(expressionData.UnderlyingExpressionData[0].Value),
                ushort.Parse(expressionData.UnderlyingExpressionData[1].Value),
                ushort.Parse(expressionData.UnderlyingExpressionData[2].Value),
                byte.Parse(expressionData.UnderlyingExpressionData[3].Value),
                byte.Parse(expressionData.UnderlyingExpressionData[4].Value),
                byte.Parse(expressionData.UnderlyingExpressionData[5].Value),
                byte.Parse(expressionData.UnderlyingExpressionData[6].Value),
                byte.Parse(expressionData.UnderlyingExpressionData[7].Value),
                byte.Parse(expressionData.UnderlyingExpressionData[8].Value),
                byte.Parse(expressionData.UnderlyingExpressionData[9].Value),
                byte.Parse(expressionData.UnderlyingExpressionData[10].Value));

            var newExpressionData = new ExpressionData(expressionData.Type, expressionData.Value, expressionData.Name, null, expressionData.TypeWithNamespace);
            var stringConstructorArgument = _primitiveExpressionGenerator.Generate(TypeCode.String, guid.ToString());
            var generated = _complexTypeInitializationGenerator.Generate(newExpressionData, new SeparatedSyntaxList<ExpressionSyntax>());
            return (new SeparatedSyntaxList<ExpressionSyntax>().Add(generated), new List<ExpressionSyntax> { stringConstructorArgument });
        }
    }
}
