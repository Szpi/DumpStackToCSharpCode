using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Generators
{
    public class ArrayCodeGenerator
    {
        public MemberDeclarationSyntax Generate(string name, ExpressionSyntax expressionSyntax)
        {
            return SyntaxFactory.FieldDeclaration(
                                    SyntaxFactory.VariableDeclaration(
                                                     SyntaxFactory.IdentifierName("var"))
                                                 .WithVariables(
                                                     SyntaxFactory.SingletonSeparatedList<
                                                         VariableDeclaratorSyntax>(
                                                         SyntaxFactory.VariableDeclarator(
                                                                          SyntaxFactory.Identifier(name))
                                                                      .WithInitializer(
                                                                          SyntaxFactory.EqualsValueClause(
                                                                              expressionSyntax)))))
                                .WithSemicolonToken(
                                    SyntaxFactory.Token(
                                        SyntaxFactory.TriviaList(),
                                        SyntaxKind.SemicolonToken,
                                        SyntaxFactory.TriviaList(
                                            SyntaxFactory.LineFeed)));
        }
    }
}