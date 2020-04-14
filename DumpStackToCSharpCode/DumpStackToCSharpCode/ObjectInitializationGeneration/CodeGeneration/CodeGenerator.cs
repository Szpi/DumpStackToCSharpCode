using DumpStackToCSharpCode.ObjectInitializationGeneration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DumpStackToCSharpCode.ObjectInitializationGeneration.Type;
using System.Collections.Generic;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.CodeGeneration
{
    public class CodeGenerator
    {
        private CompilationUnitSyntax _compilationUnitSyntax;
        private readonly VariableDeclarationManager _variableDeclarationManager;
        public CodeGenerator(VariableDeclarationManager variableDeclarationManager)
        {
            _compilationUnitSyntax = SyntaxFactory.CompilationUnit();
            _variableDeclarationManager = variableDeclarationManager;
        }

        public void AddOneExpression(string name, string type, ExpressionSyntax objectInitializationSyntax)
        {

            _compilationUnitSyntax = _compilationUnitSyntax.AddMembers(SyntaxFactory.FieldDeclaration(
                                       SyntaxFactory.VariableDeclaration(
                                               SyntaxFactory.IdentifierName(_variableDeclarationManager.GetDeclarationType(type)))
                                           .WithVariables(
                                               SyntaxFactory.SingletonSeparatedList<
                                                   VariableDeclaratorSyntax>(
                                                   SyntaxFactory.VariableDeclarator(
                                                           SyntaxFactory.Identifier(name))
                                                       .WithInitializer(
                                                           SyntaxFactory.EqualsValueClause(objectInitializationSyntax)))))
                                                                           .WithSemicolonToken(
                                                                               SyntaxFactory.Token(
                                                                                   SyntaxFactory.TriviaList(),
                                                                                   SyntaxKind.SemicolonToken,
                                                                                   SyntaxFactory.TriviaList(
                                                                                       SyntaxFactory.LineFeed))));
        }

        public void AddOnePrimitiveExpression(string name, string type, ExpressionSyntax expression)
        {
            _compilationUnitSyntax = _compilationUnitSyntax.AddMembers(SyntaxFactory.FieldDeclaration(
                                                   SyntaxFactory.VariableDeclaration(
                                                           SyntaxFactory.IdentifierName(
                                                               SyntaxFactory.Identifier(
                                                                   SyntaxFactory.TriviaList(),
                                                                   _variableDeclarationManager.GetDeclarationType(type),
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

        public string GetStringDump(MemberDeclarationSyntax memberDeclarationSyntax)
        {
            var compilationUnitSyntax = SyntaxFactory.CompilationUnit();
            compilationUnitSyntax = compilationUnitSyntax.AddMembers(memberDeclarationSyntax);

            var workspace = new AdhocWorkspace();
            var formattedSyntax = Microsoft.CodeAnalysis.Formatting.Formatter.Format(compilationUnitSyntax, workspace);
            return formattedSyntax.ToFullString();
        }
    }
}