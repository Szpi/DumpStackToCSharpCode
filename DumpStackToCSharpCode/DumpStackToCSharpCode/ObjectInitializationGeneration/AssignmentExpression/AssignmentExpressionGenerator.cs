using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.AssignmentExpression
{
    public class AssignmentExpressionGenerator
    {

        public ExpressionSyntax GenerateAssignmentExpression(string name, ExpressionSyntax expressionSyntax)
        {
            return SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                SyntaxFactory.IdentifierName(
                    SyntaxFactory.Identifier(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Tab),
                        name,
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Space))), expressionSyntax);
        }
    }
}