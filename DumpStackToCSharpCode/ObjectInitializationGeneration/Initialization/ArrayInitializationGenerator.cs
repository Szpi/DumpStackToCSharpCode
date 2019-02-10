using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ObjectInitializationGeneration.CodeGeneration;

namespace ObjectInitializationGeneration.Initialization
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