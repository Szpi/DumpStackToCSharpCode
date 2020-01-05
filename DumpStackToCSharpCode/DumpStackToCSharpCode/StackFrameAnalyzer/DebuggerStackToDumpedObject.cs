using EnvDTE80;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using RuntimeTestDataCollector.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RuntimeTestDataCollector.StackFrameAnalyzer
{
    public class DebuggerStackToDumpedObject
    {
        public async Task<(IReadOnlyList<DumpedObjectToCsharpCode> dumpedObjectToCsharpCode, string errorMessage)> DumpObjectOnStackAsync(DTE2 dte,
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

            var objectsOnStack = await debuggerStackFrameAnalyzer.AnalyzeCurrentStackAsync(dte, token);
            if (objectsOnStack.FirstOrDefault()?.ExpressionData == null)
            {
                return (new List<DumpedObjectToCsharpCode>(), objectsOnStack.FirstOrDefault().ErrorMessage);
            }

            var codeGeneratorManager = CodeGeneratorManagerFactory.Create();

            var generationTime = Stopwatch.StartNew();
            var dumpedObjectsToCsharpCode = new List<DumpedObjectToCsharpCode>();
            var errorMessage = string.Empty;

            foreach (var objectOnStack in objectsOnStack)
            {
                var expressionData = objectOnStack.ExpressionData;
                var currentExpressionDataInCSharpCode = codeGeneratorManager.GenerateStackDump(expressionData);
                dumpedObjectsToCsharpCode.Add(new DumpedObjectToCsharpCode(
                    expressionData.Name,
                    currentExpressionDataInCSharpCode, objectOnStack.ErrorMessage));

                if (!string.IsNullOrEmpty(objectOnStack.ErrorMessage))
                {
                    errorMessage = objectOnStack.ErrorMessage;
                }
            }

            Trace.WriteLine($">>>>>>>>>>>> ^^^^^^ total time seconds {generationTime.Elapsed.TotalSeconds}");
            return (dumpedObjectsToCsharpCode, errorMessage);
        }
    }
}