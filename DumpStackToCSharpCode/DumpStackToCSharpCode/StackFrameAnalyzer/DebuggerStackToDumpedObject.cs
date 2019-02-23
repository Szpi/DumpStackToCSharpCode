using System.Collections.Generic;
using EnvDTE80;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using RuntimeTestDataCollector.Window;

namespace RuntimeTestDataCollector.StackFrameAnalyzer
{
    public class DebuggerStackToDumpedObject
    {
        public IReadOnlyList<DumpedObjectToCsharpCode> DumpObjectOnStack(DTE2 dte, int maxDepth, bool generateTypeWithNamespace)
        {
            var debuggerStackFrameAnalyzer = new DebuggerStackFrameAnalyzer(maxDepth, new ConcreteTypeAnalyzer(), generateTypeWithNamespace);
            var currentExpressionData = debuggerStackFrameAnalyzer.AnalyzeCurrentStack(dte);

            var codeGeneratorManager = CodeGeneratorManagerFactory.Create();

            var dumpedObjectsToCsharpCode = new List<DumpedObjectToCsharpCode>();
            foreach (var expressionData in currentExpressionData)
            {
                var currentExpressionDataInCSharpCode = codeGeneratorManager.GenerateStackDump(new[] { expressionData });
                dumpedObjectsToCsharpCode.Add(new DumpedObjectToCsharpCode(expressionData.Name, currentExpressionDataInCSharpCode));
            }

            return dumpedObjectsToCsharpCode;
        }
    }
}