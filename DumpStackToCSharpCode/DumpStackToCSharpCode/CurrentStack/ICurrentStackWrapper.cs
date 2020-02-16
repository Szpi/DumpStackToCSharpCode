using System.Collections.Generic;
using EnvDTE80;

namespace DumpStackToCSharpCode.CurrentStack
{
    public interface ICurrentStackWrapper
    {
        IReadOnlyCollection<CurrentExpressionOnStack> CurrentExpressionOnStacks { get; }

        IReadOnlyCollection<CurrentExpressionOnStack> RefreshCurrentLocals(DTE2 dte);
        void Reset();
    }
}