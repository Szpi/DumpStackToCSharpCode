using EnvDTE;
using EnvDTE80;
using System.Collections.Generic;

namespace RuntimeTestDataCollector.CodeGeneration
{
    public class DebuggerStackFrameAnalyzer
    {
        public IReadOnlyList<ExpressionData> AnalyzeCurrentStack(DTE2 dte)
        {
            var currentStackExpressionsData = new List<ExpressionData>();
            foreach (Expression expression in dte.Debugger.CurrentStackFrame.Locals)
            {
                var expressionData = IterateThroughExpressionsData(expression);
                currentStackExpressionsData.Add(expressionData);
            }

            return currentStackExpressionsData;
        }

        private ExpressionData IterateThroughExpressionsData(Expression expression)
        {
            if (expression.DataMembers.Count == 0)
            {
                return GetExpressionData(expression);
            }

            var expressionsData = new List<ExpressionData>();
            foreach (Expression dataMember in expression.DataMembers)
            {
                if (string.IsNullOrEmpty(dataMember.Type))
                {
                    continue;
                }

                var deepestExpression = IterateThroughExpressionsData(dataMember);
                expressionsData.Add(deepestExpression);
            }

            return new ExpressionData(expression.Type, expression.Value, expression.Name, expressionsData);
        }

        private ExpressionData GetExpressionData(Expression expression)
        {
            return new ExpressionData(expression.Type, expression.Value, expression.Name, new List<ExpressionData>());
        }
    }
}