using EnvDTE;

namespace DumpStackToCSharpCode.CurrentStack
{
    public class CurrentExpressionOnStack
    {
        public string Name { get; set; }
        public Expression Expression { get; set; }
    }
}
