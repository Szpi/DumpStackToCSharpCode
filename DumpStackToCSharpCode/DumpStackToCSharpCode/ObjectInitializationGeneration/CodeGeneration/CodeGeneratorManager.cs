using DumpStackToCSharpCode.ObjectInitializationGeneration;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Generators;
using DumpStackToCSharpCode.ObjectInitializationGeneration.Initialization;
using DumpStackToCSharpCode.ObjectInitializationGeneration.Type;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration
{
    public class CodeGeneratorManager
    {
        private readonly TypeAnalyzer _typeAnalyzer;
        private readonly InitializationManager _initializationManager;
        private readonly ArrayCodeGenerator _arrayCodeGenerator;
        private readonly VariableDeclarationManager _variableDeclarationType;
        public CodeGeneratorManager(
            TypeAnalyzer typeAnalyzer, 
            InitializationManager initializationManager, 
            ArrayCodeGenerator arrayCodeGenerator,
            VariableDeclarationManager variableDeclarationType)
        {
            _typeAnalyzer = typeAnalyzer;
            _initializationManager = initializationManager;
            _arrayCodeGenerator = arrayCodeGenerator;
            _variableDeclarationType = variableDeclarationType;
        }

        public string GenerateStackDump(ExpressionData expressionsData)
        {
            var codeGenerator = new CodeGenerator(_variableDeclarationType);

            var (generatedSyntax, expressionTypeCode) = _initializationManager.GenerateForMainObject(expressionsData);

            if (_typeAnalyzer.IsPrimitiveType(expressionTypeCode))
            {
                codeGenerator.AddOnePrimitiveExpression(expressionsData.Name, expressionsData.Type, generatedSyntax);
                return codeGenerator.GetStringDump();
            }

            if (expressionTypeCode == TypeCode.Array)
            {
                var memberSyntax = _arrayCodeGenerator.Generate(expressionsData.Name, expressionsData.Type, generatedSyntax);
                return codeGenerator.GetStringDump(memberSyntax);
            }

            codeGenerator.AddOneExpression(expressionsData.Name, expressionsData.Type, generatedSyntax);

            return codeGenerator.GetStringDump();
        }
    }
}