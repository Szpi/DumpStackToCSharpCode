using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization
{
    public class ImmutableInitializationGenerator
    {
        public ExpressionSyntax Generate(ExpressionData expressionData, List<ExpressionSyntax> constructorArguments)
        {
            return SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName(expressionData.Type))
                                .WithNewKeyword(
                                    SyntaxFactory.Token(
                                        SyntaxFactory.TriviaList(),
                                        SyntaxKind.NewKeyword,
                                        SyntaxFactory.TriviaList(
                                            SyntaxFactory.Space)))
                                .WithArgumentList(GenerateArgumentListSyntaxWithCommas(constructorArguments));
        }
        private static ArgumentListSyntax GenerateArgumentListSyntaxWithCommas(List<ExpressionSyntax> argumentSyntax)
        {
            var argumentListSyntaxWithCommas = new List<SyntaxNodeOrToken>();
            for (var i = 0; i < argumentSyntax?.Count; i++)
            {
                argumentListSyntaxWithCommas.Add(SyntaxFactory.Argument(argumentSyntax[i]));
                if (i != argumentSyntax.Count - 1)
                {
                    argumentListSyntaxWithCommas.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                }
            }

            return SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(argumentListSyntaxWithCommas));
        }
    }
}