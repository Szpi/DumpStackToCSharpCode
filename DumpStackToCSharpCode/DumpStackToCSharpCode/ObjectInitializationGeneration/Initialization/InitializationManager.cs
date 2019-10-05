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

        public InitializationManager(TypeAnalyzer typeAnalyzer,
                                     PrimitiveExpressionGenerator primitiveExpressionGenerator,
                                     DictionaryExpressionGenerator dictionaryExpressionGenerator,
                                     ComplexTypeInitializationGenerator complexTypeInitializationGenerator,
                                     ArrayInitializationGenerator arrayInitializationGenerator,
                                     AssignmentExpressionGenerator assignmentExpressionGenerator,
                                     ArgumentListManager argumentListManager,
                                     EnumExpressionGenerator enumExpressionGenerator,
                                     ImmutableInitializationGenerator immutableInitializationGenerator)
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
        }

        public (SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, TypeCode mainTypeCode, List<ExpressionSyntax> argumentSyntax) Generate(ExpressionData expressionData)
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
                generatedExpressionsSyntax = generatedExpressionsSyntax.Add(generatedUnderlyingExpression);
            }

            return (generatedExpressionsSyntax, typeCode, null);
        }
        class test
        {
            public ExpressionSyntax ExpressionSyntax { get; set; }
            public TypeCode ParentTypeCode { get; set; }
        }

        private ExpressionSyntax GenerateInternal(ExpressionData expressionData, TypeCode parentTypeCode)
        {
            //var queue = new Queue<(ExpressionData expressionData, ExpressionSyntax parentExpressionSyntax)>((int)(400 * 1.2f));
            var queue = new Queue<(ExpressionData expressionData, test parentExpressionSyntax)>((int)(400 * 1.2f));

            queue.Enqueue((expressionData, null));
            test mainExpressionSyntax = null;

            bool mainExpression = true;
            while (queue.Count > 0)
            {
                var currentItem = queue.Dequeue();
                var currentExpressionData = currentItem.expressionData;
                ExpressionSyntax currentGeneratedSyntax = null;
                var (success, typeCode, (generatedSyntax, argumentList)) = GenerateForMainExpression(currentExpressionData);
                if (success)
                {
                    currentGeneratedSyntax = GenerateTest(parentTypeCode, currentExpressionData, generatedSyntax, argumentList);
                }


                //var underlyingExpressionData = IterateThroughUnderlyingExpressionsData(expressionData.UnderlyingExpressionData, typeCode);
                //var expressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
                //foreach (var expressionData in expressionsData.Where(x => x != null))
                //{
                //    var generatedUnderlyingExpression = GenerateInternal(expressionData, parentType);
                //    expressionsSyntax = expressionsSyntax.Add(generatedUnderlyingExpression);
                //}

                //return expressionsSyntax;
                //var underlyingExpressionData = IterateThroughUnderlyingExpressionsData(expressionData.UnderlyingExpressionData, typeCode);

                switch (typeCode)
                {
                    case TypeCode.DictionaryKeyValuePair:
                        {
                            var dictionaryKey = GetExpressionDataForDictionary(currentExpressionData, "Key");
                            var dictionaryValue = GetExpressionDataForDictionary(currentExpressionData, "Value");

                            var keyExpressionSyntax = GenerateInternal(dictionaryKey, typeCode);
                            var valueExpressionSyntax = GenerateInternal(dictionaryValue, typeCode);

                            //return _dictionaryExpressionGenerator.Generate(currentExpressionData, keyExpressionSyntax, valueExpressionSyntax);
                            var generated = _dictionaryExpressionGenerator.Generate(currentExpressionData, keyExpressionSyntax, valueExpressionSyntax);
                            //currentItem.parentExpressionSyntax = currentItem.parentExpressionSyntax.Add(generated);
                            currentGeneratedSyntax = generated;
                            //currentItem.parentExpressionSyntax = currentItem.parentExpressionSyntax.Add(generated);
                            var test = currentItem.parentExpressionSyntax?.ExpressionSyntax as ObjectCreationExpressionSyntax;
                            if (test == null)
                            {
                                var test1 = currentItem.parentExpressionSyntax?.ExpressionSyntax as AssignmentExpressionSyntax;
                                test = test1?.Right as ObjectCreationExpressionSyntax;
                                if (test == null)
                                {
                                    break;
                                }

                                var testInitializer = test.Initializer.AddExpressions(currentGeneratedSyntax);
                                currentItem.parentExpressionSyntax.ExpressionSyntax = test1.WithRight(test.WithInitializer(testInitializer));

                                break;
                            }

                            if (test != null)
                            {
                                var testInitializer = test.Initializer.AddExpressions(currentGeneratedSyntax);
                                currentItem.parentExpressionSyntax.ExpressionSyntax = test.WithInitializer(testInitializer);
                            }
                            break;
                        }
                    case TypeCode.Array:
                        {
                            //var arraySyntax = _arrayInitializationGenerator.Generate(expressionData, underlyingExpressionData, typeCode);
                            var arraySyntax = _arrayInitializationGenerator.Generate(currentExpressionData, new SeparatedSyntaxList<ExpressionSyntax>(), typeCode);
                            //return _assignmentExpressionGenerator.GenerateAssignmentExpression(currentExpressionData.Name, arraySyntax);
                            var generated = _assignmentExpressionGenerator.GenerateAssignmentExpression(currentExpressionData.Name, arraySyntax);
                            //currentItem.parentExpressionSyntax = currentItem.parentExpressionSyntax.Add(generated);
                            currentGeneratedSyntax = generated;
                            //currentItem.parentExpressionSyntax = currentItem.parentExpressionSyntax.Add(generated);
                            var test = currentItem.parentExpressionSyntax?.ExpressionSyntax as ObjectCreationExpressionSyntax;
                            if (test == null)
                            {
                                var test1 = currentItem.parentExpressionSyntax?.ExpressionSyntax as AssignmentExpressionSyntax;
                                test = test1?.Right as ObjectCreationExpressionSyntax;
                                if (test == null)
                                {
                                    break;
                                }

                                var testInitializer = test.Initializer.AddExpressions(currentGeneratedSyntax);
                                currentItem.parentExpressionSyntax.ExpressionSyntax = test1.WithRight(test.WithInitializer(testInitializer));

                                break;
                            }

                            if (test != null)
                            {
                                var testInitializer = test.Initializer.AddExpressions(currentGeneratedSyntax);
                                currentItem.parentExpressionSyntax.ExpressionSyntax = test.WithInitializer(testInitializer);
                            }
                            break;
                        }
                    case TypeCode.Collection:
                    case TypeCode.ComplexObject:
                    
                        {
                            //var complexTypeExpression = _complexTypeInitializationGenerator.Generate(expressionData, underlyingExpressionData);
                            var complexTypeExpression = _complexTypeInitializationGenerator.Generate(currentExpressionData, new SeparatedSyntaxList<ExpressionSyntax>());
                            //return GenerateExpressionSyntax(currentExpressionData, parentTypeCode, complexTypeExpression);
                            var generated = GenerateExpressionSyntax(currentExpressionData, currentItem.parentExpressionSyntax?.ParentTypeCode ?? parentTypeCode, complexTypeExpression);
                            currentGeneratedSyntax = generated;
                            //currentItem.parentExpressionSyntax = currentItem.parentExpressionSyntax.Add(generated);
                            var test = currentItem.parentExpressionSyntax?.ExpressionSyntax as ObjectCreationExpressionSyntax;
                            if (test == null)
                            {
                                var test1 = currentItem.parentExpressionSyntax?.ExpressionSyntax as AssignmentExpressionSyntax;
                                test = test1?.Right as ObjectCreationExpressionSyntax;
                                if (test == null)
                                {
                                    break;
                                }

                                var testInitializer = test.Initializer.AddExpressions(currentGeneratedSyntax);
                                currentItem.parentExpressionSyntax.ExpressionSyntax = test1.WithRight(test.WithInitializer(testInitializer));

                                break;
                            }

                            if (test != null)
                            {
                                var testInitializer = test.Initializer.AddExpressions(currentGeneratedSyntax);
                                currentItem.parentExpressionSyntax.ExpressionSyntax = test.WithInitializer(testInitializer);                                
                            }

                            break;
                        }
                    default:
                        {
                            //return GenerateExpressionSyntax(currentExpressionData, parentTypeCode, complexTypeExpression);
                            var generated = GenerateExpressionSyntax(currentExpressionData, currentItem.parentExpressionSyntax?.ParentTypeCode ?? parentTypeCode, currentGeneratedSyntax);
                            currentGeneratedSyntax = generated;
                            //currentItem.parentExpressionSyntax = currentItem.parentExpressionSyntax.Add(generated);
                            var test = currentItem.parentExpressionSyntax?.ExpressionSyntax as ObjectCreationExpressionSyntax;
                            if (test == null)
                            {
                                var test1 = currentItem.parentExpressionSyntax?.ExpressionSyntax as AssignmentExpressionSyntax;
                                test = test1?.Right as ObjectCreationExpressionSyntax;
                                if (test == null)
                                {
                                    break;
                                }

                                var testInitializer = test.Initializer.AddExpressions(currentGeneratedSyntax);
                                currentItem.parentExpressionSyntax.ExpressionSyntax = test1.WithRight(test.WithInitializer(testInitializer));

                                break;
                            }

                            if (test != null)
                            {
                                var testInitializer = test.Initializer.AddExpressions(currentGeneratedSyntax);
                                currentItem.parentExpressionSyntax.ExpressionSyntax = test.WithInitializer(testInitializer);
                            }
                            break;
                        }
                }

                test parent = null;
                if (mainExpression)
                {
                    mainExpression = false;
                    mainExpressionSyntax = new test { ExpressionSyntax = currentGeneratedSyntax , ParentTypeCode = typeCode } ;
                    parent = mainExpressionSyntax;
                }
                else
                {
                    ///currentItem.parentExpressionSyntax = currentItem.parentExpressionSyntax.(generatedSyntax);
                }
                parent = parent == null ? new test { ExpressionSyntax = currentGeneratedSyntax, ParentTypeCode = typeCode } : parent ;

                foreach (var child in currentExpressionData.UnderlyingExpressionData)
                {
                    queue.Enqueue((child, parent));
                }
            }

            return mainExpressionSyntax.ExpressionSyntax;
        }

        private ExpressionSyntax GenerateTest(TypeCode parentTypeCode, ExpressionData currentExpressionData, SeparatedSyntaxList<ExpressionSyntax> generatedSyntax, List<ExpressionSyntax> argumentList)
        {
            var generated = generatedSyntax.FirstOrDefault();
            if (IsNotImmutableType(generated))
            {
                return generated;
            }

            if (_typeAnalyzer.IsPrimitiveType(parentTypeCode))
            {
                return argumentList.First();
            }

            return _immutableInitializationGenerator.Generate(currentExpressionData, argumentList);
            var objectCreationSyntax = _immutableInitializationGenerator.Generate(currentExpressionData, argumentList);

            return GenerateExpressionSyntax(currentExpressionData, parentTypeCode, objectCreationSyntax);
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
                                         .Select(x => Generate(x).generatedSyntax.First())
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
                return expressionData;
            }
            return expressionData.UnderlyingExpressionData.First(x => x.Name == keyOrValueName);
        }

        private SeparatedSyntaxList<ExpressionSyntax> IterateThroughUnderlyingExpressionsData(IReadOnlyList<ExpressionData> expressionsData, TypeCode parentType)
        {
            var expressionsSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
            foreach (var expressionData in expressionsData.Where(x => x != null))
            {
                var generatedUnderlyingExpression = GenerateInternal(expressionData, parentType);
                expressionsSyntax = expressionsSyntax.Add(generatedUnderlyingExpression);
            }

            return expressionsSyntax;
        }
    }
}