using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Generators
{
    public class PrimitiveCodeGenerator
    {
        public MemberDeclarationSyntax Generate(string name, ExpressionSyntax expression)
        {
            return SyntaxFactory.FieldDeclaration(
                                    SyntaxFactory.VariableDeclaration(
                                                     SyntaxFactory.IdentifierName(
                                                         SyntaxFactory.Identifier(
                                                             SyntaxFactory.TriviaList(),
                                                             "var",
                                                             SyntaxFactory.TriviaList(
                                                                 SyntaxFactory.Space))))
                                                 .WithVariables(
                                                     SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                                         SyntaxFactory.VariableDeclarator(
                                                                          SyntaxFactory.Identifier(
                                                                              SyntaxFactory.TriviaList(),
                                                                              name,
                                                                              SyntaxFactory.TriviaList(
                                                                                  SyntaxFactory.Space)))
                                                                      .WithInitializer(
                                                                          SyntaxFactory.EqualsValueClause(expression)
                                                                                       .WithEqualsToken(
                                                                                           SyntaxFactory.Token(
                                                                                               SyntaxFactory
                                                                                                   .TriviaList(),
                                                                                               SyntaxKind
                                                                                                   .EqualsToken,
                                                                                               SyntaxFactory
                                                                                                   .TriviaList(
                                                                                                       SyntaxFactory
                                                                                                           .Space)))))))
                                                                                .WithSemicolonToken(
                                                                                    SyntaxFactory.Token(
                                                                                        SyntaxFactory.TriviaList(),
                                                                                        SyntaxKind.SemicolonToken,
                                                                                        SyntaxFactory.TriviaList(
                                                                                            SyntaxFactory.LineFeed)));
        }
    }
}