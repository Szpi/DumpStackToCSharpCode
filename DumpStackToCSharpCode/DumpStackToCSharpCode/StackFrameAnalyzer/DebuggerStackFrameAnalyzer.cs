using EnvDTE;
using EnvDTE80;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System.Collections.Generic;

namespace RuntimeTestDataCollector.StackFrameAnalyzer
{
    public class DebuggerStackFrameAnalyzer
    {
        private readonly int _maxObjectDepth;
        private readonly ConcreteTypeAnalyzer _concreteTypeAnalyzer;
        private readonly bool _generateTypeWithNamespace;

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

            foreach (Expression expression in dte.Debugger.CurrentStackFrame.Locals)
            {
                var result = IterateThroughExpressionsData(expression, 0);
                if (result.currentDepth >= _maxObjectDepth)
                {
                    continue;
                }

                currentStackExpressionsData.Add(result.ExpressionData);
            }

            return currentStackExpressionsData;
        }

        private (ExpressionData ExpressionData, int currentDepth) IterateThroughExpressionsData(Expression expression, int depth)
        {
            if (expression == null || depth == _maxObjectDepth)
            {
                return (null, depth);
            }

            depth++;
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

                var deepestResult = IterateThroughExpressionsData(dataMember, depth);
                if (deepestResult.currentDepth >= _maxObjectDepth)
                {
                    break;
                }

                expressionsData.Add(deepestResult.ExpressionData);
            }

            return (new ExpressionData(GetTypeToGenerate(expression.Type), expression.Value, expression.Name, expressionsData,expression.Type), depth );
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
            return new ExpressionData(GetTypeToGenerate(expression.Type), expression.Value, expression.Name, new List<ExpressionData>(), expression.Type);
        }
    }
}