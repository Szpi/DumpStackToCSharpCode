using EnvDTE80;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using RuntimeTestDataCollector.Window;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeTestDataCollector.StackFrameAnalyzer
{
    public class DebuggerStackToDumpedObject
    {
        public async Task<IReadOnlyList<DumpedObjectToCsharpCode>> DumpObjectOnStackAsync(DTE2 dte, int maxDepth, bool generateTypeWithNamespace)
        {
            var debuggerStackFrameAnalyzer = new DebuggerStackFrameAnalyzer(maxDepth, new ConcreteTypeAnalyzer(), generateTypeWithNamespace);
            var currentExpressionData = await debuggerStackFrameAnalyzer.AnalyzeCurrentStackAsync(dte);

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