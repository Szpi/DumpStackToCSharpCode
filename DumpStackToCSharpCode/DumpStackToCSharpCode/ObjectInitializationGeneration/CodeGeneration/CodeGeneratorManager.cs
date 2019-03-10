using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Generators;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration
{
    public class CodeGeneratorManager
    {
        private readonly TypeAnalyzer _typeAnalyzer;
        private readonly InitializationManager _initializationManager;

        public CodeGeneratorManager(TypeAnalyzer typeAnalyzer, InitializationManager initializationManager)
        {
            _typeAnalyzer = typeAnalyzer;
            _initializationManager = initializationManager;
        }

        public string GenerateStackDump(ExpressionData expressionsData)
        {
            var codeGenerator = new CodeGenerator(_typeAnalyzer);

            var (generatedSyntax, expressionTypeCode, argumentSyntax) = _initializationManager.Generate(expressionsData);

            if (_typeAnalyzer.IsPrimitiveType(expressionTypeCode))
            {
                codeGenerator.AddOnePrimitiveExpression(expressionsData.Name, generatedSyntax.FirstOrDefault());
                return codeGenerator.GetStringDump();
            }

            if (expressionTypeCode == TypeCode.Array)
            {
                var arrayInitializationGenerator = new ArrayCodeGenerator();

                var memberSyntax = arrayInitializationGenerator.Generate(expressionsData.Type, expressionsData.Name, generatedSyntax);
                return codeGenerator.GetStringDump(expressionsData.Name, memberSyntax);
            }

            codeGenerator.AddOneExpression(expressionsData.Type, expressionsData.Name, generatedSyntax, argumentSyntax);

            return codeGenerator.GetStringDump();
        }
    }
}