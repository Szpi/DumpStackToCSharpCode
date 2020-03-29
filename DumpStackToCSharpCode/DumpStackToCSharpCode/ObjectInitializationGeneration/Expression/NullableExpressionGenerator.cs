using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.Expression
{
    public class NullableExpressionGenerator
    {
        public ExpressionSyntax Generate(ExpressionSyntax primitiveExpressionSyntax, string type)
        {
            return ObjectCreationExpression(
                                GenericName(
                                    Identifier("Nullable"))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SingletonSeparatedList<TypeSyntax>(
                                          IdentifierName(type.TrimEnd('?'))))))
                            .WithArgumentList(
                                ArgumentList(
                                    SingletonSeparatedList<ArgumentSyntax>(
                                        Argument(primitiveExpressionSyntax))));
        }
    }
}
