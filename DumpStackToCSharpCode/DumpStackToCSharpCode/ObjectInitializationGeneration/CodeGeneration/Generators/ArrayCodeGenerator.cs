using System.Collections.Generic;
using DumpStackToCSharpCode.ObjectInitializationGeneration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration.Generators
{
    public class ArrayCodeGenerator
    {
        private readonly VariableDeclarationManager _variableDeclarationManager;

        public ArrayCodeGenerator(VariableDeclarationManager variableDeclarationManager)
        {
            _variableDeclarationManager = variableDeclarationManager;
        }

        public MemberDeclarationSyntax Generate(string name, string type, ExpressionSyntax expressionSyntax)
        {
            return SyntaxFactory.FieldDeclaration(
                                    SyntaxFactory.VariableDeclaration(
                                                     SyntaxFactory.IdentifierName(_variableDeclarationManager.GetDeclarationType(type)))
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