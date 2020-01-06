using EnvDTE;
using EnvDTE80;
using System.Collections.Generic;

namespace DumpStackToCSharpCode.CurrentStack
{
    public class CurrentStackWrapper : ICurrentStackWrapper
    {
        public IReadOnlyCollection<CurrentExpressionOnStack> CurrentExpressionOnStacks { get; private set; }
        public IReadOnlyCollection<CurrentExpressionOnStack> RefreshCurrentLocals(DTE2 dte)
        {
            var locals = dte?.Debugger?.CurrentStackFrame?.Locals;

            if (locals == null)
            {
                return new List<CurrentExpressionOnStack>();
            }

            var list = new List<CurrentExpressionOnStack>();

            foreach (Expression expression in locals)
            {
                list.Add(new CurrentExpressionOnStack { Expression = expression, Name = expression.Name });
            }
            CurrentExpressionOnStacks = list;
            return CurrentExpressionOnStacks;
        }
    }
}
