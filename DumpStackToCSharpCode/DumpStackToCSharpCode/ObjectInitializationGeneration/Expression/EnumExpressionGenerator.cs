using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.Expression
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