using DumpStackToCSharpCode.ObjectInitializationGeneration.Expression;
using DumpStackToCSharpCode.ObjectInitializationGeneration.Initialization;
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
        private readonly GuidInitializationManager _guidInitializationManager;
        private readonly RegexInitializationManager _regexInitializationManager;
        private readonly PrimitiveNullableExpressionGenerator _primitiveNullableExpressionGenerator;

        public InitializationManager(TypeAnalyzer typeAnalyzer,
                                     PrimitiveExpressionGenerator primitiveExpressionGenerator,
                                     DictionaryExpressionGenerator dictionaryExpressionGenerator,
                                     ComplexTypeInitializationGenerator complexTypeInitializationGenerator,
                                     ArrayInitializationGenerator arrayInitializationGenerator,
                                     AssignmentExpressionGenerator assignmentExpressionGenerator,
                                     ArgumentListManager argumentListManager,
                                     EnumExpressionGenerator enumExpressionGenerator,
                                     ImmutableInitializationGenerator immutableInitializationGenerator,
                                     ObjectInicializationExpressionGenerator objectInicializationExpressionGenerator,
                                     GuidInitializationManager guidInitializationManager,
                                     RegexInitializationManager regexInitializationManager,
                                     PrimitiveNullableExpressionGenerator primitiveNullableExpressionGenerator)
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
            _guidInitializationManager = guidInitializationManager;
            _regexInitializationManager = regexInitializationManager;
            _primitiveNullableExpressionGenerator = primitiveNullableExpressionGenerator;
        }

        public (ExpressionSyntax generatedSyntax, TypeCode mainTypeCode) GenerateForMainObject(ExpressionData expressionData, bool isParentImmutableType = false)
        {
            var (generatedSyntax, expressionTypeCode, argumentSyntax) = Generate(expressionData, isParentImmutableType);

            ExpressionSyntax generatedSyntaxForMainObject = null;
            if (_typeAnalyzer.IsNullableType(expressionData.Type))
            {
                generatedSyntaxForMainObject = _primitiveNullableExpressionGenerator.Generate(generatedSyntax, argumentSyntax, expressionData.Type);
            }
            else if (_typeAnalyzer.IsPrimitiveType(expressionTypeCode))
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

        private (SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, TypeCode mainTypeCode, List<ExpressionSyntax> argumentSyntax) Generate(ExpressionData expressionData, bool isParentImmutableType)
        {
            var (success, typeCode, valueTuple) = GenerateForMainExpression(expressionData);

            if (success)
            {
                return (valueTuple.generatedSyntax, typeCode, valueTuple.argumentSyntax);
            }

            var generatedExpressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
            foreach (var expressionDataIterator in expressionData.UnderlyingExpressionData)
            {
                var generatedUnderlyingExpression = GenerateInternal(expressionDataIterator, typeCode, isParentImmutableType);
                if (generatedUnderlyingExpression == null)
                {
                    continue;
                }

                generatedExpressionsSyntax = generatedExpressionsSyntax.Add(generatedUnderlyingExpression);
            }

            return (generatedExpressionsSyntax, typeCode, null);
        }


        private ExpressionSyntax GenerateInternal(ExpressionData expressionData, TypeCode parentTypeCode, bool isParentImmutableType)
        {
            var (success, typeCode, (generatedSyntax, argumentList)) = GenerateForMainExpression(expressionData);
            if (success)
            {
                var generated = generatedSyntax.FirstOrDefault();
                if (IsNotImmutableType(generated, typeCode))
                {
                    return GenerateExpressionSyntax(expressionData, generated, isParentImmutableType, parentTypeCode);
                }

                if (_typeAnalyzer.IsPrimitiveType(parentTypeCode))
                {
                    return argumentList.First();
                }

                var objectCreationSyntax = _immutableInitializationGenerator.Generate(expressionData, argumentList);

                return GenerateExpressionSyntax(expressionData, objectCreationSyntax, isParentImmutableType, parentTypeCode);
            }

            var underlyingExpressionData = IterateThroughUnderlyingExpressionsData(expressionData.UnderlyingExpressionData, typeCode, isParentImmutableType);

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

                        var keyExpressionSyntax = GenerateInternal(dictionaryKey, typeCode, isParentImmutableType);
                        var valueExpressionSyntax = GenerateInternal(dictionaryValue, typeCode, isParentImmutableType);

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
                        return GenerateExpressionSyntax(expressionData, complexTypeExpression, isParentImmutableType, parentTypeCode);
                    }
            }
        }

        private ExpressionSyntax GenerateExpressionSyntax(ExpressionData expressionData, ExpressionSyntax objectCreationSyntax, bool isParentImmutableType, TypeCode parentTypeCode)
        {
            return isParentImmutableType || parentTypeCode != TypeCode.ComplexObject
                ? objectCreationSyntax
                : _assignmentExpressionGenerator.GenerateAssignmentExpression(expressionData.Name, objectCreationSyntax);
        }

        private static bool IsNotImmutableType(ExpressionSyntax generated, TypeCode typeCode)
        {
            return generated != null && typeCode != TypeCode.Guid;
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

            if (typeCode == TypeCode.Enum)
            {
                var enumSyntax = _enumExpressionGenerator.Generate(expressionData);
                return (true, TypeCode.Enum, (new SeparatedSyntaxList<ExpressionSyntax>().Add(enumSyntax), null));
            }

            if (typeCode == TypeCode.Guid)
            {
                var guidSyntax = _guidInitializationManager.Generate(expressionData);
                return (true, TypeCode.Guid, guidSyntax);
            }

            if (typeCode == TypeCode.Regex)
            {
                var guidSyntax = _regexInitializationManager.Generate(expressionData);
                return (true, TypeCode.Regex, guidSyntax);
            }

            if (_typeAnalyzer.IsPrimitiveType(typeCode))
            {
                var primitiveExpression = _primitiveExpressionGenerator.Generate(typeCode, expressionData.Value);
                return (true, typeCode, (new SeparatedSyntaxList<ExpressionSyntax>().Add(primitiveExpression), null));
            }

            var immutableArgumentsList = _argumentListManager.GetArgumentList(expressionData, type);

            if (immutableArgumentsList != null)
            {
                var argumentSyntaxList = immutableArgumentsList
                                         .UnderlyingExpressionData
                                         .Select(x => GenerateForMainObject(x, true).generatedSyntax)
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
            var type = System.Type.GetType(expressionData.TypeWithNamespace.TrimEnd('?'));
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

        private SeparatedSyntaxList<ExpressionSyntax> IterateThroughUnderlyingExpressionsData(IReadOnlyList<ExpressionData> expressionsData, TypeCode parentType, bool isParentImmutableType)
        {
            var expressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
            foreach (var expressionData in expressionsData.Where(x => x != null))
            {
                var generatedUnderlyingExpression = GenerateInternal(expressionData, parentType, isParentImmutableType);
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