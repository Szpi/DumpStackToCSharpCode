using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Expression
{
    public class DictionaryExpressionGenerator
    {   
        public ExpressionSyntax Generate(ExpressionSyntax keyExpressionSyntax, ExpressionSyntax valueExpressionSyntax)
        {
            
            return SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                                SyntaxFactory.ImplicitElementAccess()
                                                .WithArgumentList(
                                                    SyntaxFactory.BracketedArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                keyExpressionSyntax)))
                                                    .WithOpenBracketToken(
                                                        SyntaxFactory.Token(
                                                            SyntaxFactory.TriviaList(SyntaxFactory.Tab),
                                                            SyntaxKind.OpenBracketToken,
                                                            SyntaxFactory.TriviaList()))
                                                    .WithCloseBracketToken(
                                                        SyntaxFactory.Token(
                                                            SyntaxFactory.TriviaList(),
                                                            SyntaxKind.CloseBracketToken,
                                                            SyntaxFactory.TriviaList(
                                                                SyntaxFactory.Space)))),
                                                valueExpressionSyntax)
                                            .WithOperatorToken(
                                                SyntaxFactory.Token(
                                                    SyntaxFactory.TriviaList(),
                                                    SyntaxKind.EqualsToken,
                                                    SyntaxFactory.TriviaList(
                                                        SyntaxFactory.Space)));
        }
    }
}