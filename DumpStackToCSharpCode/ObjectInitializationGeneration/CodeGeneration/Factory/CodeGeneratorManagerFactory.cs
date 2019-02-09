using ObjectInitializationGeneration.Type;

namespace ObjectInitializationGeneration.CodeGeneration.Factory
{
    public class CodeGeneratorManagerFactory
    {
        public static CodeGeneratorManager Create()
        {
            return new CodeGeneratorManager(new ExpressionSyntaxGenerator(new TypeAnalyzer()), new TypeAnalyzer());
        }
    }
}