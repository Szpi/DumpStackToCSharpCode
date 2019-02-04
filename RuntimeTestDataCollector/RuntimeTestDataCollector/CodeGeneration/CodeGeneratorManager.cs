using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace RuntimeTestDataCollector.CodeGeneration
{
    public class CodeGeneratorManager
    {
        private readonly ExpressionSyntaxGenerator _expressionSyntaxGenerator;
        private readonly TypeAnalyzer _typeAnalyzer;

        public CodeGeneratorManager(ExpressionSyntaxGenerator expressionSyntaxGenerator, TypeAnalyzer typeAnalyzer)
        {
            _expressionSyntaxGenerator = expressionSyntaxGenerator;
            _typeAnalyzer = typeAnalyzer;
        }

        public string GenerateStackDump(IReadOnlyList<ExpressionData> expressionsData)
        {
            var codeGenerator = new CodeGenerator();

            foreach (var expression in expressionsData)
            {
                if (_typeAnalyzer.IsPrimitiveType(expression.Type))
                {
                    var generatedPrimitiveExpression = _expressionSyntaxGenerator.GenerateSyntaxForPrimitiveExpression(expression);
                    codeGenerator.AddOnePrimitiveExpression(expression.Name, generatedPrimitiveExpression);
                    continue;
                }

                var generatedExpressionsData = IterateThroughUnderlyingExpressionsData(expression.UnderlyingExpressionData);
                codeGenerator.AddOneExpression(expression.Type, expression.Name, generatedExpressionsData);
            }

            return codeGenerator.GetStringDump();
        }

        private SeparatedSyntaxList<ExpressionSyntax> IterateThroughUnderlyingExpressionsData(IReadOnlyList<ExpressionData> expressionsData)
        {
            var expressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
            foreach (var expressionData in expressionsData)
            {
                expressionsSyntax = expressionsSyntax.Add(IterateThroughExpressionsData(expressionData, string.Empty));
            }

            return expressionsSyntax;
        }

        private ExpressionSyntax IterateThroughExpressionsData(ExpressionData expression, string parentType)
        {
            if (expression.UnderlyingExpressionData.Count == 0)
            {
                if (_typeAnalyzer.IsCollection(parentType))
                {
                    return _expressionSyntaxGenerator.GenerateSyntaxForPrimitiveExpression(expression);
                }
                return _expressionSyntaxGenerator.GenerateAssignmentExpressionForPrimitiveType(expression);
            }

            var expressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
            foreach (var underlyingExpressionData in expression.UnderlyingExpressionData)
            {
                var deepestExpression = IterateThroughExpressionsData(underlyingExpressionData, expression.Type);
                expressionsSyntax = expressionsSyntax.Add(deepestExpression);
            }

            return _expressionSyntaxGenerator.GenerateAssignmentExpressionForComplexExpressionSyntax(expression, expressionsSyntax);
        }
    }
}