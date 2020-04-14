using DumpStackToCSharpCode.Command.Util;
using DumpStackToCSharpCode.ObjectInitializationGeneration;
using DumpStackToCSharpCode.ObjectInitializationGeneration.AssignmentExpression;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Generators;
using DumpStackToCSharpCode.ObjectInitializationGeneration.Constructor;
using DumpStackToCSharpCode.ObjectInitializationGeneration.Expression;
using DumpStackToCSharpCode.ObjectInitializationGeneration.Initialization;
using DumpStackToCSharpCode.ObjectInitializationGeneration.Type;
using System.Collections.Generic;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Factory
{
    public class CodeGeneratorManagerFactory
    {
        public static CodeGeneratorManager Create(Dictionary<string, IReadOnlyList<string>> readonlyObjects, bool useConcreteType)
        {
            var argumentListManager = new ArgumentListManager(readonlyObjects, new ConcreteTypeAnalyzer(), new DumpStackToCSharpCode.ObjectInitializationGeneration.Constructor.ConstructorsManager());

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
            
            return new CodeGeneratorManager(new TypeAnalyzer(), initializationManager, new ArrayCodeGenerator(new VariableDeclarationManager(useConcreteType)), new VariableDeclarationManager(useConcreteType));
        }
    }
}