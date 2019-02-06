using EnvDTE;
using EnvDTE80;
using System.Collections.Generic;

namespace RuntimeTestDataCollector.CodeGeneration
{
    public class DebuggerStackFrameAnalyzer
    {
        private readonly int _maxObjectDepth;

        public DebuggerStackFrameAnalyzer(int maxObjectDepth)
        {
            _maxObjectDepth = maxObjectDepth;
        }

        public IReadOnlyList<ExpressionData> AnalyzeCurrentStack(DTE2 dte)
        {
            var currentStackExpressionsData = new List<ExpressionData>();
            if (dte?.Debugger == null || dte.Debugger.CurrentStackFrame == null)
            {
                return currentStackExpressionsData;
            }

            foreach (Expression expression in dte.Debugger.CurrentStackFrame.Locals)
            {
                var currentDepth = 0;
                var result = IterateThroughExpressionsData(expression, ref currentDepth);
                if (!result.Success)
                {
                    continue;
                }

                currentStackExpressionsData.Add(result.ExpressionData);
            }

            return currentStackExpressionsData;
        }

        private (ExpressionData ExpressionData, bool Success) IterateThroughExpressionsData(Expression expression, ref int depth)
        {
            if (expression == null || depth == _maxObjectDepth)
            {
                return (null, false);
            }

            depth++;
            if (expression.DataMembers.Count == 0)
            {
                return (GetExpressionData(expression), true);
            }

            var expressionsData = new List<ExpressionData>();
            foreach (Expression dataMember in expression.DataMembers)
            {
                if (dataMember == null || string.IsNullOrEmpty(dataMember.Type))
                {
                    continue;
                }

                var deepestResult = IterateThroughExpressionsData(dataMember, ref depth);
                if (!deepestResult.Success)
                {
                    continue;
                }

                expressionsData.Add(deepestResult.ExpressionData);
            }

            return (new ExpressionData(expression.Type, expression.Value, expression.Name, expressionsData), true);
        }

        private ExpressionData GetExpressionData(Expression expression)
        {
            return new ExpressionData(expression.Type, expression.Value, expression.Name, new List<ExpressionData>());
        }
    }
}