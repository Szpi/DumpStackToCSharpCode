using DumpStackToCSharpCode.ObjectInitializationGeneration.Expression;
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
        private readonly ImmutableInitializationGenerator _immutableInitializationGenerator;
        private readonly ObjectInicializationExpressionGenerator _objectInicializationExpressionGenerator;

        public InitializationManager(TypeAnalyzer typeAnalyzer,
                                     PrimitiveExpressionGenerator primitiveExpressionGenerator,
                                     DictionaryExpressionGenerator dictionaryExpressionGenerator,
                                     ComplexTypeInitializationGenerator complexTypeInitializationGenerator,
                                     ArrayInitializationGenerator arrayInitializationGenerator,
                                     AssignmentExpressionGenerator assignmentExpressionGenerator,
                                     ArgumentListManager argumentListManager,
                                     EnumExpressionGenerator enumExpressionGenerator,
                                     ImmutableInitializationGenerator immutableInitializationGenerator,
                                     ObjectInicializationExpressionGenerator objectInicializationExpressionGenerator)
        {
            _typeAnalyzer = typeAnalyzer;
            _primitiveExpressionGenerator = primitiveExpressionGenerator;
            _dictionaryExpressionGenerator = dictionaryExpressionGenerator;
            _complexTypeInitializationGenerator = complexTypeInitializationGenerator;
            _arrayInitializationGenerator = arrayInitializationGenerator;
            _assignmentExpressionGenerator = assignmentExpressionGenerator;
            _argumentListManager = argumentListManager;
            _enumExpressionGenerator = enumExpressionGenerator;
            _immutableInitializationGenerator = immutableInitializationGenerator;
            _objectInicializationExpressionGenerator = objectInicializationExpressionGenerator;
        }

        public (ExpressionSyntax generatedSyntax, TypeCode mainTypeCode) GenerateForMainObject(ExpressionData expressionData)
        {
            var (generatedSyntax, expressionTypeCode, argumentSyntax) = Generate(expressionData);

            ExpressionSyntax generatedSyntaxForMainObject = null;
            if (_typeAnalyzer.IsPrimitiveType(expressionTypeCode))
            {
                generatedSyntaxForMainObject = generatedSyntax.FirstOrDefault();
            }
            else if (expressionTypeCode == TypeCode.Array)
            {
                generatedSyntaxForMainObject = _objectInicializationExpressionGenerator.GenerateForArray(expressionData.Type, generatedSyntax);
            }
            else
            {
                generatedSyntaxForMainObject = _objectInicializationExpressionGenerator.GenerateForObject(expressionData.Type, generatedSyntax, argumentSyntax);
            }

            return (generatedSyntaxForMainObject, expressionTypeCode);
        }

        private (SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, TypeCode mainTypeCode, List<ExpressionSyntax> argumentSyntax) Generate(ExpressionData expressionData)
        {
            var (success, typeCode, valueTuple) = GenerateForMainExpression(expressionData);

            if (success)
            {
                return (valueTuple.generatedSyntax, typeCode, valueTuple.argumentSyntax);
            }

            var generatedExpressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
            foreach (var expressionDataIterator in expressionData.UnderlyingExpressionData)
            {
                var generatedUnderlyingExpression = GenerateInternal(expressionDataIterator, typeCode);
                if (generatedUnderlyingExpression == null)
                {
                    continue;
                }

                generatedExpressionsSyntax = generatedExpressionsSyntax.Add(generatedUnderlyingExpression);
            }

            return (generatedExpressionsSyntax, typeCode, null);
        }


        private ExpressionSyntax GenerateInternal(ExpressionData expressionData, TypeCode parentTypeCode)
        {
            var (success, typeCode, (generatedSyntax, argumentList)) = GenerateForMainExpression(expressionData);
            if (success)
            {
                var generated = generatedSyntax.FirstOrDefault();
                if (IsNotImmutableType(generated))
                {
                    return GenerateExpressionSyntax(expressionData, parentTypeCode, generated);
                }

                if (_typeAnalyzer.IsPrimitiveType(parentTypeCode))
                {
                    return argumentList.First();
                }

                var objectCreationSyntax = _immutableInitializationGenerator.Generate(expressionData, argumentList);

                return GenerateExpressionSyntax(expressionData, parentTypeCode, objectCreationSyntax);
            }

            var underlyingExpressionData = IterateThroughUnderlyingExpressionsData(expressionData.UnderlyingExpressionData, typeCode);

            switch (typeCode)
            {
                case TypeCode.DictionaryKeyValuePair:
                    {
                        var dictionaryKey = GetExpressionDataForDictionary(expressionData, "Key");
                        if (dictionaryKey == null)
                        {
                            return null;
                        }
                        
                        var dictionaryValue = GetExpressionDataForDictionary(expressionData, "Value");

                        if (dictionaryValue == null)
                        {
                            return null;
                        }

                        var keyExpressionSyntax = GenerateInternal(dictionaryKey, typeCode);
                        var valueExpressionSyntax = GenerateInternal(dictionaryValue, typeCode);

                        return _dictionaryExpressionGenerator.Generate(keyExpressionSyntax, valueExpressionSyntax);
                    }
                case TypeCode.Array:
                    {
                        var arraySyntax = _arrayInitializationGenerator.Generate(expressionData, underlyingExpressionData, typeCode);
                        return _assignmentExpressionGenerator.GenerateAssignmentExpression(expressionData.Name, arraySyntax);
                    }
                case TypeCode.Collection:
                default:
                    {
                        var complexTypeExpression = _complexTypeInitializationGenerator.Generate(expressionData, underlyingExpressionData);
                        return GenerateExpressionSyntax(expressionData, parentTypeCode, complexTypeExpression);
                    }
            }
        }

        private ExpressionSyntax GenerateExpressionSyntax(ExpressionData expressionData, TypeCode parentTypeCode, ExpressionSyntax objectCreationSyntax)
        {
            return parentTypeCode == TypeCode.ComplexObject
                ? _assignmentExpressionGenerator.GenerateAssignmentExpression(expressionData.Name, objectCreationSyntax)
                : objectCreationSyntax;
        }

        private static bool IsNotImmutableType(ExpressionSyntax generated)
        {
            return generated != null;
        }

        private (bool success, TypeCode typeCode, (SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, List<ExpressionSyntax> argumentSyntax) valueTuple)
            GenerateForMainExpression(ExpressionData expressionData)
        {
            var typeCode = _typeAnalyzer.GetTypeCode(expressionData.TypeWithNamespace, expressionData.Value);
            var (success, generatedValueTuple, type) = TryGenerateForBuiltInEnum(expressionData, typeCode);

            if (success)
            {
                return (true, TypeCode.Enum, generatedValueTuple);
            }

            if (_typeAnalyzer.IsPrimitiveType(expressionData.TypeWithNamespace, expressionData.Value))
            {
                var primitiveExpression = _primitiveExpressionGenerator.Generate(typeCode, expressionData.Value);
                {
                    return (true, typeCode, (new SeparatedSyntaxList<ExpressionSyntax>().Add(primitiveExpression), null));
                }
            }

            var immutableArgumentsList = _argumentListManager.GetArgumentList(expressionData, type);

            if (immutableArgumentsList != null)
            {
                var argumentSyntaxList = immutableArgumentsList
                                         .UnderlyingExpressionData
                                         .Select(x => GenerateForMainObject(x).generatedSyntax)
                                         .Where(x => x != null)
                                         .ToList();

                return (true, typeCode, (new SeparatedSyntaxList<ExpressionSyntax>(), argumentSyntaxList));
            }

            return (false, typeCode, (new SeparatedSyntaxList<ExpressionSyntax>(), null));
        }

        private (bool success,
                (SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, List<ExpressionSyntax> argumentSyntax) valueTuple,
                System.Type type)
                TryGenerateForBuiltInEnum(ExpressionData expressionData, TypeCode typeCode)
        {
            if (typeCode != TypeCode.ComplexObject)
            {
                return (false, (new SeparatedSyntaxList<ExpressionSyntax>(), null), null);
            }
            try
            {
                return TryGetBuiltInDotNetType(expressionData);
            }
            catch (System.Exception)
            {
                return (false, (new SeparatedSyntaxList<ExpressionSyntax>(), null), null);
            }
        }

        private (bool success,
                (SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, List<ExpressionSyntax> argumentSyntax)
                valueTuple,
                System.Type type) TryGetBuiltInDotNetType(ExpressionData expressionData)
        {
            var type = System.Type.GetType(expressionData.TypeWithNamespace);
            if (type == null || !type.IsEnum)
            {
                return (false, (new SeparatedSyntaxList<ExpressionSyntax>(), null), type);
            }

            var enumExpression = _enumExpressionGenerator.Generate(expressionData);

            return (true, (new SeparatedSyntaxList<ExpressionSyntax>().Add(enumExpression), null), type);
        }

        private static ExpressionData GetExpressionDataForDictionary(ExpressionData expressionData, string keyOrValueName)
        {
            if (expressionData.UnderlyingExpressionData.Count == 0)
            {
                return null;
            }
            return expressionData.UnderlyingExpressionData.First(x => x.Name == keyOrValueName);
        }

        private SeparatedSyntaxList<ExpressionSyntax> IterateThroughUnderlyingExpressionsData(IReadOnlyList<ExpressionData> expressionsData, TypeCode parentType)
        {
            var expressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
            foreach (var expressionData in expressionsData.Where(x => x != null))
            {
                var generatedUnderlyingExpression = GenerateInternal(expressionData, parentType);
                if (generatedUnderlyingExpression == null)
                {
                    continue;
                }
                expressionsSyntax = expressionsSyntax.Add(generatedUnderlyingExpression);
            }

            return expressionsSyntax;
        }
    }
}