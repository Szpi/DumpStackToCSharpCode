using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.Expression
{
    public class PrimitiveNullableExpressionGenerator
    {
        private readonly ObjectInicializationExpressionGenerator _objectInicializationExpressionGenerator;

        public PrimitiveNullableExpressionGenerator(ObjectInicializationExpressionGenerator objectInicializationExpressionGenerator)
        {
            _objectInicializationExpressionGenerator = objectInicializationExpressionGenerator;
        }

        public ExpressionSyntax Generate(SeparatedSyntaxList<ExpressionSyntax> expressions, List<ExpressionSyntax> argumentSyntax, string type)
        {
            if(argumentSyntax?.Count == 1)
            {
                return Generate(argumentSyntax.First(), type);
            }

            if (expressions.Count == 1)
            {
                return Generate(expressions.First(), type);
            }

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
                                        Argument(_objectInicializationExpressionGenerator.GenerateForObject(type.TrimEnd('?'), SeparatedList<ExpressionSyntax>(), argumentSyntax)))));
        }
        private ExpressionSyntax Generate(ExpressionSyntax primitiveExpressionSyntax, string type)
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
