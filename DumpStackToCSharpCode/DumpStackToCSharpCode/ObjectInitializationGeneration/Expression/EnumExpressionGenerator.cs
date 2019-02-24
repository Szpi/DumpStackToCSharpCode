using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Expression
{
    public class EnumExpressionGenerator
    {
        public ExpressionSyntax Generate(ExpressionData expressionData)
        {
           return SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName(expressionData.Type),
                SyntaxFactory.IdentifierName(expressionData.Value));
        }
    }
}