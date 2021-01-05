using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration;

namespace DumpStackToCSharpCode.StackFrameAnalyzer
{
    public class ObjectOnStack
    {
        public ExpressionData ExpressionData { get; }
        public string ErrorMessage { get; }
               
        public ObjectOnStack(ExpressionData expressionData, string errorMessage)
        {
            ExpressionData = expressionData;
            ErrorMessage = errorMessage;
        }
    }
}
