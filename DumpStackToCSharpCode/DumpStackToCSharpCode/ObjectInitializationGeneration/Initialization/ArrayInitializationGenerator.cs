using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization
{
    public class ArrayInitializationGenerator
    {
        public ExpressionSyntax Generate(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            return SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName(expressionData.Type))
                                .WithNewKeyword(
                                    SyntaxFactory.Token(
                                        SyntaxFactory.TriviaList(),
                                        SyntaxKind.NewKeyword,
                                        SyntaxFactory.TriviaList(
                                            SyntaxFactory.Space))).WithInitializer(
                                    SyntaxFactory.InitializerExpression(
                                        SyntaxKind.ObjectInitializerExpression, expressionsSyntax));
        }
    }
}