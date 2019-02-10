using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

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

        public void AddOneExpression(string type, string name, SeparatedSyntaxList<ExpressionSyntax> expressions)
        {
           ArgumentListSyntax argumentList = null;
      
            if (!_typeAnalyzer.IsArray(type))
            {
                argumentList =
                    SyntaxFactory.ArgumentList().WithCloseParenToken(
                        SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(),
                            SyntaxKind.CloseParenToken,
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.LineFeed)));
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
                                                                   .WithInitializer(
                                                                       SyntaxFactory.InitializerExpression(
                                                                           SyntaxKind
                                                                               .ObjectInitializerExpression,
                                                                           expressions)))))))
                                                                           .WithSemicolonToken(
                                                                               SyntaxFactory.Token(
                                                                                   SyntaxFactory.TriviaList(),
                                                                                   SyntaxKind.SemicolonToken,
                                                                                   SyntaxFactory.TriviaList(
                                                                                       SyntaxFactory.LineFeed))));
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