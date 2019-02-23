using RuntimeTestDataCollector.ObjectInitializationGeneration.AssignmentExpression;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Constructor;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Expression;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System.Collections.Generic;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory
{
    public class CodeGeneratorManagerFactory
    {
        public static CodeGeneratorManager Create()
        {
            var arguments = new Dictionary<string, IReadOnlyList<string>>
            {
                ["DateTime"] = new List<string>() { "Year", "Month", "Day", "Hour", "Minutes" }
            };
            var argumentListManager = new ArgumentListManager(arguments, new ConcreteTypeAnalyzer());

            var initializationManager = new InitializationManager(new TypeAnalyzer(),
                                                                  new PrimitiveExpressionGenerator(),
                                                                  new DictionaryExpressionGenerator(),
                                                                  new ComplexTypeInitializationGenerator(new TypeAnalyzer()),
                                                                  new ArrayInitializationGenerator(),
                                                                  new AssignmentExpressionGenerator(),
                                                                  argumentListManager);
            ;
            return new CodeGeneratorManager(new TypeAnalyzer(), initializationManager);
        }
    }
}