using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Generators
{
    public class ComplexObjectCodeGenerator
    {
        public MemberDeclarationSyntax Generate(string type,
                                                string name,
                                                SeparatedSyntaxList<ExpressionSyntax> expressions,
                                                List<ExpressionSyntax> argumentSyntax)
        {
            var argumentList = GenerateArgumentListSyntaxWithCommas(argumentSyntax);
            InitializerExpressionSyntax initializerExpressionSyntax = null;

            if (argumentList.Arguments.Count == 0)
            {
                argumentList = CreateEmptyArgumentList();

                initializerExpressionSyntax =
                    SyntaxFactory.InitializerExpression(
                        SyntaxKind
                            .ObjectInitializerExpression,
                        expressions);
            }

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
                                                                              SyntaxFactory.ObjectCreationExpression(
                                                                                               SyntaxFactory
                                                                                                   .IdentifierName(
                                                                                                       type))
                                                                                           .WithArgumentList(
                                                                                               argumentList)
                                                                                           .WithInitializer(
                                                                                               initializerExpressionSyntax))))))
                                .WithSemicolonToken(
                                    SyntaxFactory.Token(
                                        SyntaxFactory.TriviaList(),
                                        SyntaxKind.SemicolonToken,
                                        SyntaxFactory.TriviaList(
                                            SyntaxFactory.LineFeed)));
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

        private static ArgumentListSyntax CreateEmptyArgumentList()
        {
            return SyntaxFactory.ArgumentList().WithCloseParenToken(
                SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.CloseParenToken,
                    SyntaxFactory.TriviaList(
                        SyntaxFactory.LineFeed)));
        }
    }
}