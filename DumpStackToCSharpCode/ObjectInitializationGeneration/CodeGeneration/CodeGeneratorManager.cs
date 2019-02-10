using System.Collections.Generic;
using ObjectInitializationGeneration.AssignmentExpression;
using ObjectInitializationGeneration.Expression;
using ObjectInitializationGeneration.Initialization;
using ObjectInitializationGeneration.Type;

namespace ObjectInitializationGeneration.CodeGeneration
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
            var codeGenerator = new CodeGenerator(_typeAnalyzer);
            var initializationManager = new InitializationManager(new TypeAnalyzer(),
                                                                  new PrimitiveExpressionGenerator(),
                                                                  new DictionaryExpressionGenerator(),
                                                                  new ComplexTypeInitializationGenerator(
                                                                      new TypeAnalyzer()),
                                                                  new ArrayInitializationGenerator(),
                                                                  new AssignmentExpressionGenerator());

            foreach (var expression in expressionsData)
            {
                //if (_typeAnalyzer.IsPrimitiveType(expression.Type))
                //{
                //    var generatedPrimitiveExpression = _expressionSyntaxGenerator.GenerateSyntaxForPrimitiveExpression(expression.Type, expression.Value);
                //    codeGenerator.AddOnePrimitiveExpression(expression.Name, generatedPrimitiveExpression);
                //    continue;
                //}

                var generatedExpressionsData = initializationManager.Generate(expression);
                codeGenerator.AddOneExpression(expression.Type, expression.Name, generatedExpressionsData);
            }

            return codeGenerator.GetStringDump();
        }

        //private SeparatedSyntaxList<ExpressionSyntax> IterateThroughUnderlyingExpressionsData(IReadOnlyList<ExpressionData> expressionsData, string parentType)
        //{
        //    var expressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
        //    foreach (var expressionData in expressionsData)
        //    {
        //        expressionsSyntax = expressionsSyntax.Add(IterateThroughExpressionsData(expressionData, parentType));
        //    }

        //    return expressionsSyntax;
        //}

        //private ExpressionSyntax IterateThroughExpressionsData(ExpressionData expression, string parentType)
        //{
        //    if (_typeAnalyzer.IsDictionaryKeyValuePair(expression.Type))
        //    {
        //        var dictionaryKey = expression.UnderlyingExpressionData.First(x => x.Name == "Key");
        //        var dictionaryValue = expression.UnderlyingExpressionData.First(x => x.Name == "Value");

        //        var keyExpressionSyntax = GenerateExpressionForUnderlyingExpressions(dictionaryKey);
        //        var valueExpressionSyntax = GenerateExpressionForUnderlyingExpressions(dictionaryValue);
        //        return _expressionSyntaxGenerator.GenerateAssignmentForDictionary(keyExpressionSyntax, valueExpressionSyntax);
        //    }

        //    if (expression.UnderlyingExpressionData.Count == 0)
        //    {
        //        if (_typeAnalyzer.IsCollection(parentType) || _typeAnalyzer.IsArray(parentType))
        //        {
        //            return _expressionSyntaxGenerator.GenerateSyntaxForPrimitiveExpression(expression.Type, expression.Value);
        //        }

        //        return _expressionSyntaxGenerator.GenerateAssignmentExpressionForPrimitiveType(expression);
        //    }

        //    return GenerateAssignmentForUnderlyingExpressions(expression);
        //}

        //private ExpressionSyntax GenerateAssignmentForUnderlyingExpressions(ExpressionData expression)
        //{
        //    var expressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
        //    foreach (var underlyingExpressionData in expression.UnderlyingExpressionData)
        //    {
        //        var deepestExpression = IterateThroughExpressionsData(underlyingExpressionData, expression.Type);
        //        expressionsSyntax = expressionsSyntax.Add(deepestExpression);
        //    }

        //    return _expressionSyntaxGenerator.GenerateAssignmentExpressionForComplexExpressionSyntax(expression, expressionsSyntax);
        //}

        //private ExpressionSyntax GenerateExpressionForUnderlyingExpressions(ExpressionData expression)
        //{
        //    if (expression.UnderlyingExpressionData.Count == 0)
        //    {
        //        return _expressionSyntaxGenerator.GenerateSyntaxForPrimitiveExpression(expression.Type, expression.Value);
        //    }

        //    var expressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
        //    foreach (var underlyingExpressionData in expression.UnderlyingExpressionData)
        //    {
        //        var deepestExpression = IterateThroughExpressionsData(underlyingExpressionData, expression.Type);
        //        expressionsSyntax = expressionsSyntax.Add(deepestExpression);
        //    }

        //    return _expressionSyntaxGenerator.GenerateSyntaxForComplexExpression(expression, expressionsSyntax);
        //}
    }
}