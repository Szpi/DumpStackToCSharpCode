using DumpStackToCSharpCode.Command.Util;
using RuntimeTestDataCollector.ObjectInitializationGeneration.AssignmentExpression;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Generators;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Constructor;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Expression;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System.Collections.Generic;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory
{
    public class CodeGeneratorManagerFactory
    {
        public static CodeGeneratorManager Create(Dictionary<string, IReadOnlyList<string>> readonlyObjects)
        {
            var argumentListManager = new ArgumentListManager(readonlyObjects, new ConcreteTypeAnalyzer());

            var initializationManager = new InitializationManager(new TypeAnalyzer(),
                                                                  new PrimitiveExpressionGenerator(),
                                                                  new DictionaryExpressionGenerator(),
                                                                  new ComplexTypeInitializationGenerator(new TypeAnalyzer()),
                                                                  new ArrayInitializationGenerator(new TypeAnalyzer()),
                                                                  new AssignmentExpressionGenerator(),
                                                                  argumentListManager,
                                                                  new EnumExpressionGenerator(),
                                                                  new ImmutableInitializationGenerator(),
                                                                  new DumpStackToCSharpCode.ObjectInitializationGeneration.Expression.ObjectInicializationExpressionGenerator(new TypeAnalyzer()),
                                                                  new DumpStackToCSharpCode.ObjectInitializationGeneration.Initialization.GuidInitializationManager(new ComplexTypeInitializationGenerator(new TypeAnalyzer()), new PrimitiveExpressionGenerator()),
                                                                  new DumpStackToCSharpCode.ObjectInitializationGeneration.Initialization.RegexInitializationManager(new ComplexTypeInitializationGenerator(new TypeAnalyzer()), new PrimitiveExpressionGenerator()),
                                                                  new DumpStackToCSharpCode.ObjectInitializationGeneration.Expression.NullableExpressionGenerator());
            
            return new CodeGeneratorManager(new TypeAnalyzer(), initializationManager, new ArrayCodeGenerator());
        }
    }
}