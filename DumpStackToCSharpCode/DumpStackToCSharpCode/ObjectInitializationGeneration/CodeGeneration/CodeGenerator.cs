using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System.Collections.Generic;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration
{
    public class CodeGenerator
    {
        private CompilationUnitSyntax _compilationUnitSyntax;
        private TypeAnalyzer _typeAnalyzer;
        public CodeGenerator(TypeAnalyzer typeAnalyzer)
        {
            _typeAnalyzer = typeAnalyzer;
            _compilationUnitSyntax = SyntaxFactory.CompilationUnit();
        }

        public void AddOneExpression(string type, string name, SeparatedSyntaxList<ExpressionSyntax> expressions,
                                     List<ExpressionSyntax> argumentSyntax)
        {
            var argumentList = GenerateArgumentListSyntaxWithCommas(argumentSyntax);
            InitializerExpressionSyntax initializerExpressionSyntax = null;

            if (!_typeAnalyzer.IsArray(type) && argumentList.Arguments.Count == 0)
            {
                argumentList =
                    SyntaxFactory.ArgumentList().WithCloseParenToken(
                        SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(),
                            SyntaxKind.CloseParenToken,
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.LineFeed)));

                initializerExpressionSyntax =
                    SyntaxFactory.InitializerExpression(
                        SyntaxKind
                            .ObjectInitializerExpression,
                        expressions);
            }

            _compilationUnitSyntax = _compilationUnitSyntax.AddMembers(SyntaxFactory.FieldDeclaration(
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
                                                                       SyntaxFactory.IdentifierName(
                                                                           type))
                                                                   .WithArgumentList(argumentList)
                                                                   .WithInitializer(initializerExpressionSyntax))))))
                                                                           .WithSemicolonToken(
                                                                               SyntaxFactory.Token(
                                                                                   SyntaxFactory.TriviaList(),
                                                                                   SyntaxKind.SemicolonToken,
                                                                                   SyntaxFactory.TriviaList(
                                                                                       SyntaxFactory.LineFeed))));
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

        public void AddOnePrimitiveExpression(string name, ExpressionSyntax expression)
        {
            _compilationUnitSyntax = _compilationUnitSyntax.AddMembers(SyntaxFactory.FieldDeclaration(
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
                                                                                   SyntaxFactory.TriviaList(),
                                                                                   SyntaxKind.EqualsToken,
                                                                                   SyntaxFactory.TriviaList(
                                                                                       SyntaxFactory.Space)))))))
                                                                           .WithSemicolonToken(
                                                                               SyntaxFactory.Token(
                                                                                   SyntaxFactory.TriviaList(),
                                                                                   SyntaxKind.SemicolonToken,
                                                                                   SyntaxFactory.TriviaList(
                                                                                       SyntaxFactory.LineFeed))));
        }
        public string GetStringDump()
        {
            var workspace = new AdhocWorkspace();
            var formattedSyntax = Microsoft.CodeAnalysis.Formatting.Formatter.Format(_compilationUnitSyntax, workspace);
            return formattedSyntax.ToFullString();
        }
    }
}