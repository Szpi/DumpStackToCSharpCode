using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RuntimeTestDataCollector.CodeGeneration
{
    public class CodeGenerator
    {
        private CompilationUnitSyntax _compilationUnitSyntax;
        private TypeAnalyzer _typeAnalyzer;
        public CodeGenerator(TypeAnalyzer typeAnalyzer)
        {
            _typeAnalyzer = typeAnalyzer;
            _compilationUnitSyntax = CompilationUnit();
        }

        public void AddOneExpression(string type, string name, SeparatedSyntaxList<ExpressionSyntax> expressions)
        {
           ArgumentListSyntax argumentList = null;
      
            if (!_typeAnalyzer.IsArray(type))
            {
                argumentList =
                    ArgumentList().WithCloseParenToken(
                        Token(
                            TriviaList(),
                            SyntaxKind.CloseParenToken,
                            TriviaList(
                                LineFeed)));
            }

            _compilationUnitSyntax = _compilationUnitSyntax.AddMembers(FieldDeclaration(
                                       VariableDeclaration(
                                               IdentifierName("var"))
                                           .WithVariables(
                                               SingletonSeparatedList<
                                                   VariableDeclaratorSyntax>(
                                                   VariableDeclarator(
                                                           Identifier(name))
                                                       .WithInitializer(
                                                           EqualsValueClause(
                                                               ObjectCreationExpression(
                                                                       IdentifierName(
                                                                           type))
                                                                   .WithArgumentList(argumentList)
                                                                   .WithInitializer(
                                                                       InitializerExpression(
                                                                           SyntaxKind
                                                                               .ObjectInitializerExpression,
                                                                           expressions)))))))
                                                                           .WithSemicolonToken(
                                                                               Token(
                                                                                   TriviaList(),
                                                                                   SyntaxKind.SemicolonToken,
                                                                                   TriviaList(
                                                                                       LineFeed))));
        }

        public void AddOnePrimitiveExpression(string name, ExpressionSyntax expression)
        {
            _compilationUnitSyntax = _compilationUnitSyntax.AddMembers(FieldDeclaration(
                                                   VariableDeclaration(
                                                           IdentifierName(
                                                               Identifier(
                                                                   TriviaList(),
                                                                   "var",
                                                                   TriviaList(
                                                                       Space))))
                                                       .WithVariables(
                                                           SingletonSeparatedList<VariableDeclaratorSyntax>(
                                                               VariableDeclarator(
                                                                       Identifier(
                                                                           TriviaList(),
                                                                           name,
                                                                           TriviaList(
                                                                               Space)))
                                                                   .WithInitializer(
                                                                       EqualsValueClause(expression)
                                                                           .WithEqualsToken(
                                                                               Token(
                                                                                   TriviaList(),
                                                                                   SyntaxKind.EqualsToken,
                                                                                   TriviaList(
                                                                                       Space)))))))
                                                                           .WithSemicolonToken(
                                                                               Token(
                                                                                   TriviaList(),
                                                                                   SyntaxKind.SemicolonToken,
                                                                                   TriviaList(
                                                                                       LineFeed))));
        }
        public string GetStringDump()
        {
            var workspace = new AdhocWorkspace();
            var formattedSyntax = Microsoft.CodeAnalysis.Formatting.Formatter.Format(_compilationUnitSyntax, workspace);
            return formattedSyntax.ToFullString();
        }
    }
}