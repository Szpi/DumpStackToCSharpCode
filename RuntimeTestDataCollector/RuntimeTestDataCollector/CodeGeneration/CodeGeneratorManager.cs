using EnvDTE;
using EnvDTE80;

namespace RuntimeTestDataCollector.CodeGeneration
{
    public class CodeGeneratorManager
    {
        public string GenerateStackDump(DTE2 dte)
        {
            var codeGenerator = new CodeGenerator();

            foreach (Expression expression in dte.Debugger.CurrentStackFrame.Locals)
            {
                codeGenerator.WithNewObject(expression.Type);
                foreach (Expression dataMember in expression.DataMembers)
                {
                    codeGenerator.WithInitializeExpression(dataMember.Name, dataMember.Type, dataMember.Value);
                }
                codeGenerator.EndMemberDeclaration();
            }

            return codeGenerator.GetStringDump();
        }
    }
}