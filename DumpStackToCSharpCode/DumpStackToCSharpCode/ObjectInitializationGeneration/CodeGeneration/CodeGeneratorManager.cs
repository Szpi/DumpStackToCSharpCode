using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Generators;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration
{
    public class CodeGeneratorManager
    {
        private readonly TypeAnalyzer _typeAnalyzer;
        private readonly InitializationManager _initializationManager;
        private readonly ArrayCodeGenerator _arrayCodeGenerator;
        public CodeGeneratorManager(TypeAnalyzer typeAnalyzer, InitializationManager initializationManager, ArrayCodeGenerator arrayCodeGenerator)
        {
            _typeAnalyzer = typeAnalyzer;
            _initializationManager = initializationManager;
            _arrayCodeGenerator = arrayCodeGenerator;
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
                var memberSyntax = _arrayCodeGenerator.Generate(expressionsData.Type, expressionsData.Name, generatedSyntax);
                return codeGenerator.GetStringDump(expressionsData.Name, memberSyntax);
            }

            codeGenerator.AddOneExpression(expressionsData.Type, expressionsData.Name, generatedSyntax, argumentSyntax);

            return codeGenerator.GetStringDump();
        }
    }
}