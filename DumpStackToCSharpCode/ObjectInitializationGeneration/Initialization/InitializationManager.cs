using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ObjectInitializationGeneration.AssignmentExpression;
using ObjectInitializationGeneration.CodeGeneration;
using ObjectInitializationGeneration.Type;
using System.Collections.Generic;
using System.Linq;
using ObjectInitializationGeneration.Expression;

namespace ObjectInitializationGeneration.Initialization
{
    public class InitializationManager
    {
        private readonly TypeAnalyzer _typeAnalyzer;
        private readonly PrimitiveExpressionGenerator _primitiveExpressionGenerator;
        private readonly DictionaryExpressionGenerator _dictionaryExpressionGenerator;
        private readonly ComplexTypeInitializationGenerator _complexTypeInitializationGenerator;
        private readonly ArrayInitializationGenerator _arrayInitializationGenerator;
        private readonly AssignmentExpressionGenerator _assignmentExpressionGenerator;

        public InitializationManager(TypeAnalyzer typeAnalyzer,
                                     PrimitiveExpressionGenerator primitiveExpressionGenerator,
                                     DictionaryExpressionGenerator dictionaryExpressionGenerator,
                                     ComplexTypeInitializationGenerator complexTypeInitializationGenerator,
                                     ArrayInitializationGenerator arrayInitializationGenerator,
                                     AssignmentExpressionGenerator assignmentExpressionGenerator)
        {
            _typeAnalyzer = typeAnalyzer;
            _primitiveExpressionGenerator = primitiveExpressionGenerator;
            _dictionaryExpressionGenerator = dictionaryExpressionGenerator;
            _complexTypeInitializationGenerator = complexTypeInitializationGenerator;
            _arrayInitializationGenerator = arrayInitializationGenerator;
            _assignmentExpressionGenerator = assignmentExpressionGenerator;
        }

        public SeparatedSyntaxList<ExpressionSyntax> Generate(ExpressionData expressionData)
        {
            var typeCode = _typeAnalyzer.GetTypeCode(expressionData.Type);

            if (_typeAnalyzer.IsPrimitiveType(expressionData.Type))
            {
                var primitiveExpression = _primitiveExpressionGenerator.Generate(typeCode, expressionData.Value);
                return new SeparatedSyntaxList<ExpressionSyntax>
                {
                    _assignmentExpressionGenerator.GenerateAssignmentExpressionForPrimitiveType(
                        expressionData.Name, primitiveExpression)
                };
            }

            var generatedExpressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
            foreach (var expressionDataIterator in expressionData.UnderlyingExpressionData)
            {
                generatedExpressionsSyntax = generatedExpressionsSyntax.Add(GenerateInternal(expressionDataIterator, typeCode));
            }

            return generatedExpressionsSyntax;
        }

        public ExpressionSyntax GenerateInternal(ExpressionData expressionData, TypeCode parentTypeCode)
        {
            var typeCode = _typeAnalyzer.GetTypeCode(expressionData.Type);

            if (_typeAnalyzer.IsPrimitiveType(expressionData.Type))
            {
                return _primitiveExpressionGenerator.Generate(typeCode, expressionData.Value);
            }

            var underlyingExpressionData = IterateThroughUnderlyingExpressionsData(expressionData.UnderlyingExpressionData, typeCode);

            if (typeCode == TypeCode.DictionaryKeyValuePair)
            {

                var dictionaryKey = expressionData.UnderlyingExpressionData.First(x => x.Name == "Key");
                var dictionaryValue = expressionData.UnderlyingExpressionData.First(x => x.Name == "Value");

                var keyExpressionSyntax = Generate(dictionaryKey);
                var valueExpressionSyntax = Generate(dictionaryValue);

                return _dictionaryExpressionGenerator.Generate(expressionData, keyExpressionSyntax.FirstOrDefault(), valueExpressionSyntax.FirstOrDefault());
            }

            if (typeCode == TypeCode.Array)
            {
                return _arrayInitializationGenerator.Generate(expressionData, underlyingExpressionData);
            }

            return _assignmentExpressionGenerator.GenerateAssignmentExpressionForPrimitiveType(expressionData.Name, _complexTypeInitializationGenerator.Generate(expressionData, underlyingExpressionData));
        }

        private SeparatedSyntaxList<ExpressionSyntax> IterateThroughUnderlyingExpressionsData(IReadOnlyList<ExpressionData> expressionsData, TypeCode parentType)
        {
            var expressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
            foreach (var expressionData in expressionsData)
            {
                expressionsSyntax = expressionsSyntax.Add(GenerateInternal(expressionData, parentType));
            }

            return expressionsSyntax;
        }
    }
}