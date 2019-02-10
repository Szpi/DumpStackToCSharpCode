using System.Collections.Generic;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration
{
    public class CodeGeneratorManager
    {
        private readonly TypeAnalyzer _typeAnalyzer;
        private readonly InitializationManager _initializationManager;

        public CodeGeneratorManager( TypeAnalyzer typeAnalyzer, InitializationManager initializationManager)
        {
            _typeAnalyzer = typeAnalyzer;
            _initializationManager = initializationManager;
        }

        public string GenerateStackDump(IReadOnlyList<ExpressionData> expressionsData)
        {
            var codeGenerator = new CodeGenerator(_typeAnalyzer);

            foreach (var expression in expressionsData)
            {
                var generatedExpressionsData = _initializationManager.Generate(expression);
                if (generatedExpressionsData.IsPrimitiveType)
                {
                    codeGenerator.AddOnePrimitiveExpression(expression.Name, generatedExpressionsData.generatedSyntax.FirstOrDefault());
                    continue;
                }
                codeGenerator.AddOneExpression(expression.Type, expression.Name, generatedExpressionsData.generatedSyntax);
            }

            return codeGenerator.GetStringDump();
        }
    }
}