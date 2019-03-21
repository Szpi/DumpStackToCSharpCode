using EnvDTE;
using EnvDTE80;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RuntimeTestDataCollector.StackFrameAnalyzer
{
    public class DebuggerStackFrameAnalyzer
    {
        private readonly int _maxObjectDepth;
        private readonly ConcreteTypeAnalyzer _concreteTypeAnalyzer;
        private readonly bool _generateTypeWithNamespace;
        private readonly TimeSpan _maxGenerationTime = TimeSpan.FromSeconds(5);

        public DebuggerStackFrameAnalyzer(int maxObjectDepth,
                                          ConcreteTypeAnalyzer concreteTypeAnalyzer,
                                          bool generateTypeWithNamespace)
        {
            _maxObjectDepth = maxObjectDepth;
            _concreteTypeAnalyzer = concreteTypeAnalyzer;
            _generateTypeWithNamespace = generateTypeWithNamespace;
        }

        public IReadOnlyList<ExpressionData> AnalyzeCurrentStack(DTE2 dte)
        {
            var currentStackExpressionsData = new List<ExpressionData>();
            if (dte?.Debugger?.CurrentStackFrame == null)
            {
                return currentStackExpressionsData;
            }

            var generationTime = Stopwatch.StartNew();
            foreach (Expression expression in dte.Debugger.CurrentStackFrame.Locals)
            {
                var (expressionData, depth) = IterateThroughExpressionsData(expression, 0, generationTime);
                if (depth >= _maxObjectDepth)
                {
                    continue;
                }

                currentStackExpressionsData.Add(expressionData);
                if (HasExceedMaxGenerationTime(generationTime))
                {
                    break;
                }
            }

            return currentStackExpressionsData;
        }

        private (ExpressionData ExpressionData, int currentDepth) IterateThroughExpressionsData(
            Expression expression, int depth, Stopwatch generationTime)
        {
            Trace.WriteLine($">>>>>>>>>>>> depth {depth}");
            if (expression == null || depth == _maxObjectDepth)
            {
                return (null, depth);
            }

            depth++;
            Trace.WriteLine($">>>>>>>>>>>> count {expression.DataMembers.Count}");
            if (expression.DataMembers.Count == 0)
            {
                return (GetExpressionData(expression), depth);
            }

            var expressionsData = new List<ExpressionData>();
            foreach (Expression dataMember in expression.DataMembers)
            {
                if (dataMember == null || string.IsNullOrEmpty(dataMember.Type))
                {
                    continue;
                }

                if (IsDictionaryDuplicatedValue(dataMember.Name))
                {
                    continue;
                }
                Trace.WriteLine($">>>>>>>>>>>> seconds {generationTime.Elapsed.TotalSeconds}");
                if (HasExceedMaxGenerationTime(generationTime))
                {
                    break;
                }

                Trace.WriteLine($">>>>>>>>>>>> datamember {dataMember.Name} {dataMember.Type} {dataMember.Value}");
                var deepestResult = IterateThroughExpressionsData(dataMember, depth, generationTime);
                if (deepestResult.currentDepth >= _maxObjectDepth)
                {
                    Trace.WriteLine($">>>>>>>>>>>> depth {depth} break");
                    break;
                }

                expressionsData.Add(deepestResult.ExpressionData);
            }

            var value = CorrectCharValue(expression.Type, expression.Value);
            return (new ExpressionData(GetTypeToGenerate(expression.Type), value, expression.Name, expressionsData, expression.Type), depth);
        }

        private bool IsDictionaryDuplicatedValue(string dataMemberType)
        {
            return dataMemberType == "type" || dataMemberType == "value";
        }

        private string GetTypeToGenerate(string type)
        {
            var concreteType = _concreteTypeAnalyzer.ParseConcreteType(type);
            return _generateTypeWithNamespace ? concreteType : _concreteTypeAnalyzer.GetTypeWithoutNamespace(concreteType);
        }

        private ExpressionData GetExpressionData(Expression expression)
        {
            var value = CorrectCharValue(expression.Type, expression.Value);
            return new ExpressionData(GetTypeToGenerate(expression.Type), value, expression.Name, new List<ExpressionData>(), expression.Type);
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

            var charStartIndex = value.IndexOf("\'") + 1;
            var charEndIndex = value.LastIndexOf("\'");

            if (charEndIndex <= 0 || charStartIndex <= 0)
            {
                return value;
            }

            return value.Substring(charStartIndex , charEndIndex - charStartIndex).Replace("\\","");
        }
    }
}