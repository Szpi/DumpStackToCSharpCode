namespace RuntimeTestDataCollector.CodeGeneration.Factory
{
    public class CodeGeneratorManagerFactory
    {
        public static CodeGeneratorManager Create()
        {
            return new CodeGeneratorManager(new ExpressionSyntaxGenerator(new TypeAnalyzer()), new TypeAnalyzer());
        }
    }
}