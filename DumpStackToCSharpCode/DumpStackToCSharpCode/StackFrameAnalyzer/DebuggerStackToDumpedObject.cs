using EnvDTE80;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using RuntimeTestDataCollector.Window;
using System.Collections.Generic;

namespace RuntimeTestDataCollector.StackFrameAnalyzer
{
    public class DebuggerStackToDumpedObject
    {
        public IReadOnlyList<DumpedObjectToCsharpCode> DumpObjectOnStack(DTE2 dte, int maxDepth, bool generateTypeWithNamespace)
        {
            var debuggerStackFrameAnalyzer = new DebuggerStackFrameAnalyzer(maxDepth, new ConcreteTypeAnalyzer(new TypeAnalyzer()), generateTypeWithNamespace);
            var currentExpressionData = debuggerStackFrameAnalyzer.AnalyzeCurrentStack(dte);

            var codeGeneratorManager = CodeGeneratorManagerFactory.Create();

            var dumpedObjectsToCsharpCode = new List<DumpedObjectToCsharpCode>();
            foreach (var expressionData in currentExpressionData)
            {
                var currentExpressionDataInCSharpCode = codeGeneratorManager.GenerateStackDump(expressionData);
                dumpedObjectsToCsharpCode.Add(new DumpedObjectToCsharpCode(expressionData.Name, currentExpressionDataInCSharpCode));
            }

            return dumpedObjectsToCsharpCode;
        }
    }
}