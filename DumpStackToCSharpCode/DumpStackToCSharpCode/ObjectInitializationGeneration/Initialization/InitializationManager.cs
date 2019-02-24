using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.AssignmentExpression;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Constructor;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Expression;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization
{
    public class InitializationManager
    {
        private readonly TypeAnalyzer _typeAnalyzer;
        private readonly PrimitiveExpressionGenerator _primitiveExpressionGenerator;
        private readonly DictionaryExpressionGenerator _dictionaryExpressionGenerator;
        private readonly ComplexTypeInitializationGenerator _complexTypeInitializationGenerator;
        private readonly ArrayInitializationGenerator _arrayInitializationGenerator;
        private readonly AssignmentExpressionGenerator _assignmentExpressionGenerator;
        private readonly ArgumentListManager _argumentListManager;
        private readonly EnumExpressionGenerator _enumExpressionGenerator;

        public InitializationManager(TypeAnalyzer typeAnalyzer,
                                     PrimitiveExpressionGenerator primitiveExpressionGenerator,
                                     DictionaryExpressionGenerator dictionaryExpressionGenerator,
                                     ComplexTypeInitializationGenerator complexTypeInitializationGenerator,
                                     ArrayInitializationGenerator arrayInitializationGenerator,
                                     AssignmentExpressionGenerator assignmentExpressionGenerator,
                                     ArgumentListManager argumentListManager,
                                     EnumExpressionGenerator enumExpressionGenerator)
        {
            _typeAnalyzer = typeAnalyzer;
            _primitiveExpressionGenerator = primitiveExpressionGenerator;
            _dictionaryExpressionGenerator = dictionaryExpressionGenerator;
            _complexTypeInitializationGenerator = complexTypeInitializationGenerator;
            _arrayInitializationGenerator = arrayInitializationGenerator;
            _assignmentExpressionGenerator = assignmentExpressionGenerator;
            _argumentListManager = argumentListManager;
            _enumExpressionGenerator = enumExpressionGenerator;
        }

        public (SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, bool IsPrimitiveType, List<ExpressionSyntax> argumentSyntax) Generate(ExpressionData expressionData)
        {
            var typeCode = _typeAnalyzer.GetTypeCode(expressionData.Type);
            var (success, generatedValueTuple, type) = TryGenerateForBuiltInEnum(expressionData, typeCode);

            if (success)
            {
                return generatedValueTuple;
            }

            if (_typeAnalyzer.IsPrimitiveType(expressionData.Type))
            {
                var primitiveExpression = _primitiveExpressionGenerator.Generate(typeCode, expressionData.Value);
                return (new SeparatedSyntaxList<ExpressionSyntax>().Add(primitiveExpression), true, null);
            }

            var immutableArgumentsList = _argumentListManager.GetArgumentList(expressionData, type);

            if (immutableArgumentsList != null)
            {
                var argumentSyntaxList = immutableArgumentsList
                                         .UnderlyingExpressionData
                                         .Select(x => Generate(x).generatedSyntax.First())
                                         .ToList();

                return (new SeparatedSyntaxList<ExpressionSyntax>(), false, argumentSyntaxList);
            }

            var generatedExpressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
            foreach (var expressionDataIterator in expressionData.UnderlyingExpressionData)
            {
                generatedExpressionsSyntax = generatedExpressionsSyntax.Add(GenerateInternal(expressionDataIterator, typeCode));
            }

            return (generatedExpressionsSyntax, false, null);
        }

        private (bool success,
                (SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, bool IsPrimitiveType, List<ExpressionSyntax> argumentSyntax) valueTuple,
                System.Type type)
                TryGenerateForBuiltInEnum(ExpressionData expressionData, TypeCode typeCode)
        {
            if (typeCode != TypeCode.ComplexObject)
            {
                return (false, (new SeparatedSyntaxList<ExpressionSyntax>(), false, null), null);
            }

            var type = System.Type.GetType(expressionData.TypeWithNamespace);
            if (type == null || !type.IsEnum)
            {
                return (false, (new SeparatedSyntaxList<ExpressionSyntax>(), false, null), type);
            }

            var enumExpression = _enumExpressionGenerator.Generate(expressionData);

            return (true, (new SeparatedSyntaxList<ExpressionSyntax>().Add(enumExpression), true, null), type);
        }

        public ExpressionSyntax GenerateInternal(ExpressionData expressionData, TypeCode parentTypeCode)
        {
            var typeCode = _typeAnalyzer.GetTypeCode(expressionData.Type);

            if (_typeAnalyzer.IsPrimitiveType(expressionData.Type))
            {
                return _primitiveExpressionGenerator.Generate(typeCode, expressionData.Value);
            }

            var underlyingExpressionData = IterateThroughUnderlyingExpressionsData(expressionData.UnderlyingExpressionData, typeCode);

            switch (typeCode)
            {
                case TypeCode.DictionaryKeyValuePair:
                    {
                        var dictionaryKey = GetExpressionDataForDictionary(expressionData, "Key");
                        var dictionaryValue = GetExpressionDataForDictionary(expressionData, "Value");

                        var keyExpressionSyntax = GenerateInternal(dictionaryKey, typeCode);
                        var valueExpressionSyntax = GenerateInternal(dictionaryValue, typeCode);

                        return _dictionaryExpressionGenerator.Generate(expressionData, keyExpressionSyntax, valueExpressionSyntax);
                    }
                case TypeCode.Array:
                    {
                        return _arrayInitializationGenerator.Generate(expressionData, underlyingExpressionData);
                    }
                default:
                    {
                        return _assignmentExpressionGenerator.GenerateAssignmentExpressionForPrimitiveType(expressionData.Name, _complexTypeInitializationGenerator.Generate(expressionData, underlyingExpressionData));
                    }
            }
        }

        private static ExpressionData GetExpressionDataForDictionary(ExpressionData expressionData, string keyOrValueName)
        {
            if (expressionData.UnderlyingExpressionData.Count == 0)
            {
                return expressionData;
            }
            return expressionData.UnderlyingExpressionData.First(x => x.Name == keyOrValueName);
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