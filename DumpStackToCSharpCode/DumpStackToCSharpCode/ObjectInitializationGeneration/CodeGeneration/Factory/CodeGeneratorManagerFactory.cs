using RuntimeTestDataCollector.ObjectInitializationGeneration.AssignmentExpression;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Expression;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory
{
    public class CodeGeneratorManagerFactory
    {
        public static CodeGeneratorManager Create()
        {
            var initializationManager = new InitializationManager(new TypeAnalyzer(),
                                                                  new PrimitiveExpressionGenerator(),
                                                                  new DictionaryExpressionGenerator(),
                                                                  new ComplexTypeInitializationGenerator(
                                                                      new TypeAnalyzer()),
                                                                  new ArrayInitializationGenerator(),
                                                                  new AssignmentExpressionGenerator());
            ;
            return new CodeGeneratorManager(new TypeAnalyzer(), initializationManager);
        }
    }
}