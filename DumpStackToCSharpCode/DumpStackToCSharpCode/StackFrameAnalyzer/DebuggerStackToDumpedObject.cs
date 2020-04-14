using DumpStackToCSharpCode.Resources;
using EnvDTE;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Factory;
using DumpStackToCSharpCode.ObjectInitializationGeneration.Type;
using DumpStackToCSharpCode.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DumpStackToCSharpCode.StackFrameAnalyzer
{
    public class DebuggerStackToDumpedObject
    {
        public (IReadOnlyList<DumpedObjectToCsharpCode> dumpedObjectToCsharpCode, string errorMessage) DumpObjectOnStack(
                                                                                          IReadOnlyCollection<Expression> currentExpressionOnStacks,
                                                                                          int maxDepth,
                                                                                          bool generateTypeWithNamespace,
                                                                                          int maxObjectsToAnalyze,
                                                                                          TimeSpan maxGenerationTime,
                                                                                          Dictionary<string, IReadOnlyList<string>> readonlyObjects,
                                                                                          bool useConcreteType)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            var debuggerStackFrameAnalyzer = new DebuggerStackFrameAnalyzer(
                                                    maxDepth,
                                                    new ConcreteTypeAnalyzer(),
                                                    generateTypeWithNamespace,
                                                    maxObjectsToAnalyze,
                                                    maxGenerationTime);


            var objectsOnStack = debuggerStackFrameAnalyzer.AnalyzeCurrentStack(currentExpressionOnStacks);
            if (objectsOnStack.FirstOrDefault()?.ExpressionData == null)
            {
                return (new List<DumpedObjectToCsharpCode>(), objectsOnStack?.FirstOrDefault()?.ErrorMessage ?? ErrorMessages.EmptyObjectOnStack);
            }

            var codeGeneratorManager = CodeGeneratorManagerFactory.Create(readonlyObjects, useConcreteType);

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