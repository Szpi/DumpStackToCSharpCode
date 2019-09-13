using EnvDTE80;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using RuntimeTestDataCollector.Window;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RuntimeTestDataCollector.StackFrameAnalyzer
{
    public class DebuggerStackToDumpedObject
    {
        public async Task<IReadOnlyList<DumpedObjectToCsharpCode>> DumpObjectOnStackAsync(DTE2 dte,
                                                                                          int maxDepth,
                                                                                          bool generateTypeWithNamespace,
                                                                                          CancellationToken token,
                                                                                          int maxObjectsToAnalyze,
                                                                                          TimeSpan maxGenerationTime)
        {
            var debuggerStackFrameAnalyzer = new DebuggerStackFrameAnalyzer(
                                                    maxDepth,
                                                    new ConcreteTypeAnalyzer(),
                                                    generateTypeWithNamespace,
                                                    maxObjectsToAnalyze,
                                                    maxGenerationTime);

            var currentExpressionData = await debuggerStackFrameAnalyzer.AnalyzeCurrentStackAsync(dte, token);

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