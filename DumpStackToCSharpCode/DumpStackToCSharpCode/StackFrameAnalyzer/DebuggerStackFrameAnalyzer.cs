using DumpStackToCSharpCode.StackFrameAnalyzer;
using EnvDTE;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RuntimeTestDataCollector.StackFrameAnalyzer
{
    public class DebuggerStackFrameAnalyzer
    {
        private static readonly string DateTime = $"{nameof(System)}.{nameof(DateTime)}";

        private readonly int _maxObjectsToAnalyze;
        private readonly int _maxObjectDepth;
        private readonly ConcreteTypeAnalyzer _concreteTypeAnalyzer;
        private readonly bool _generateTypeWithNamespace;
        private readonly TimeSpan _maxGenerationTime;

        public DebuggerStackFrameAnalyzer(int maxObjectDepth,
                                          ConcreteTypeAnalyzer concreteTypeAnalyzer,
                                          bool generateTypeWithNamespace,
                                          int maxObjectsToAnalyze,
                                          TimeSpan maxGenerationTime)
        {
            _maxObjectDepth = maxObjectDepth;
            _concreteTypeAnalyzer = concreteTypeAnalyzer;
            _generateTypeWithNamespace = generateTypeWithNamespace;
            _maxObjectsToAnalyze = maxObjectsToAnalyze;
            _maxGenerationTime = maxGenerationTime;
        }

        public IReadOnlyList<ObjectOnStack> AnalyzeCurrentStack(IReadOnlyCollection<Expression> currentExpressionOnStacks)
        {
            var generationTime = Stopwatch.StartNew();
            int currentAnalyzedObject = 0;
            var currentStackExpressionsData = new List<ObjectOnStack>();

            foreach (Expression expression in currentExpressionOnStacks)
            {
                if (currentAnalyzedObject > _maxObjectsToAnalyze)
                {
                    break;
                }

                var objectOnStack = GenerateExpressionData(expression, ref currentAnalyzedObject, generationTime);

                if (objectOnStack == null)
                {
                    continue;
                }

                currentStackExpressionsData.Add(objectOnStack);

                if (HasExceedMaxGenerationTime(generationTime))
                {
                    Trace.WriteLine($">>>>>>>>>>>> seconds {generationTime.Elapsed.TotalSeconds} breaking !!");
                    break;
                }
            }

            Trace.WriteLine($">>>>>>>>>>>> |||||||||||||||| total time seconds {generationTime.Elapsed.TotalSeconds}");

            return currentStackExpressionsData;
        }

        private ObjectOnStack GenerateExpressionData(Expression expression, ref int overallAnalyzedObjects, Stopwatch generationTime)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            if (expression == null)
            {
                return null;
            }

            var queue = new Queue<(Expression expression, List<ExpressionData> parentExpressionData)>((int)(_maxObjectsToAnalyze * 1.2f));

            queue.Enqueue((expression, null));
            ExpressionData mainObject = null;
            var currentObjectDepth = 1;

            var depthEndsAfterAnalyzingObjects = 1;
            var nextDepthAfterAnalyzedObjects = depthEndsAfterAnalyzingObjects;
            var currentAnalyzedObjects = 0;

            while (queue.Count > 0)
            {
                var stackObject = queue.Dequeue();
                var dataMember = stackObject.expression;

                currentAnalyzedObjects++;
                overallAnalyzedObjects++;

                if (HasExceedMaxGenerationTime(generationTime))
                {
                    Trace.WriteLine($">>>>>>>>>>>> seconds {generationTime.Elapsed.TotalSeconds} breaking");
                    return new ObjectOnStack(mainObject, DumpStackToCSharpCode.Resources.ErrorMessages.GenerationTimeExceeded);
                }

                if (currentAnalyzedObjects > _maxObjectsToAnalyze)
                {
                    return new ObjectOnStack(mainObject, DumpStackToCSharpCode.Resources.ErrorMessages.MaxObjectToAnalyzeExceeded);
                }

                if (currentObjectDepth > _maxObjectDepth)
                {
                    return new ObjectOnStack(mainObject, DumpStackToCSharpCode.Resources.ErrorMessages.MaxObjectDepthExceeded);
                }
                var dataMemberName = dataMember.Name;
                var dataMemberType = dataMember.Type;

                if (IsDictionaryDuplicatedValue(dataMemberName))
                {
                    continue;
                }

                //if (IsDateTimeRecursion(dataMemberType, currentObjectDepth))
                //{
                //    continue;
                //}

                var underlyingExpressionData = new List<ExpressionData>();

                var value = CorrectCharValue(dataMemberType, dataMember.Value);
                var type = GetTypeToGenerate(dataMemberType);

                var expressionData = new ExpressionData(type, value, dataMemberName, underlyingExpressionData, dataMemberType);

                if (HasParentExpressionData(stackObject))
                {
                    stackObject.parentExpressionData.Add(expressionData);
                }
                else
                {
                    mainObject = expressionData;
                }
                var remainingObjectToAnalyze = _maxObjectsToAnalyze - currentAnalyzedObjects;

                if (remainingObjectToAnalyze <= 0)
                {
                    continue;
                }

                var validChildren = dataMember.DataMembers.Cast<Expression>().Where(x => !string.IsNullOrEmpty(x?.Type)).ToList();
                int iteration = 0;

                foreach (var child in validChildren)
                {
                    if (iteration > remainingObjectToAnalyze)
                    {
                        break;
                    }
                    iteration++;
                    queue.Enqueue((child, underlyingExpressionData));
                }

                nextDepthAfterAnalyzedObjects += iteration;

                if (WasCurrentDepthReached(currentAnalyzedObjects, depthEndsAfterAnalyzingObjects))
                {
                    depthEndsAfterAnalyzingObjects = nextDepthAfterAnalyzedObjects;
                    currentObjectDepth++;
                }
            }

            return new ObjectOnStack(mainObject, null);
        }

        private bool IsDateTimeRecursion(string dataMemberType, int currentDepth)
        {
            return dataMemberType == DateTime && currentDepth >= 3;
        }

        private static bool WasCurrentDepthReached(int currentAnalyzedObjects, int depthEndsAfterAnalyzingObjects)
        {
            return currentAnalyzedObjects == depthEndsAfterAnalyzingObjects;
        }

        private static bool HasParentExpressionData((Expression expression, List<ExpressionData> parentExpressionData) stackObject)
        {
            return stackObject.parentExpressionData != null;
        }

        private bool IsDictionaryDuplicatedValue(string dataMemberType)
        {
            return dataMemberType == "type" || dataMemberType == "value";
        }

        private string GetTypeToGenerate(string type)
        {
            var concreteType = _concreteTypeAnalyzer.ParseConcreteType(type);
            return _generateTypeWithNamespace
                ? concreteType
                : _concreteTypeAnalyzer.GetTypeWithoutNamespace(concreteType);
        }

        private bool HasExceedMaxGenerationTime(Stopwatch generationTime)
        {
            return generationTime.Elapsed > _maxGenerationTime;
        }

        private string CorrectCharValue(string type, string value)
        {
            if (type != "char")
            {
                return value;
            }

            var charStartIndex = value.IndexOf("\'");

            return value.Substring(charStartIndex);
        }
    }
}