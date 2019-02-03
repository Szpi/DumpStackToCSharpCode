using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RuntimeTestDataCollector.CodeGeneration
{
    public class CodeGenerator
    {
        private CompilationUnitSyntax _compilationUnitSyntax;
        public CodeGenerator()
        {
            _compilationUnitSyntax = CompilationUnit();
        }

        public void AddOneExpression(string type, SeparatedSyntaxList<ExpressionSyntax> expressions)
        {
            _compilationUnitSyntax = _compilationUnitSyntax.AddMembers(FieldDeclaration(
                                       VariableDeclaration(
                                               IdentifierName("var"))
                                           .WithVariables(
                                               SingletonSeparatedList<
                                                   VariableDeclaratorSyntax>(
                                                   VariableDeclarator(
                                                           Identifier(
                                                               FirstToLowerWithoutComma(
                                                                   type)))
                                                       .WithInitializer(
                                                           EqualsValueClause(
                                                               ObjectCreationExpression(
                                                                       IdentifierName(
                                                                           type))
                                                                   .WithArgumentList(
                                                                       ArgumentList().WithCloseParenToken(
                                                                           Token(
                                                                               TriviaList(),
                                                                               SyntaxKind.CloseParenToken,
                                                                               TriviaList(
                                                                                   LineFeed))))
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
        public string GetStringDump()
        {
            var workspace = new AdhocWorkspace();
            var formattedSyntax = Microsoft.CodeAnalysis.Formatting.Formatter.Format(_compilationUnitSyntax, workspace);
            return formattedSyntax.ToFullString();
        }
      
        private string FirstToLowerWithoutComma(string @string)
        {
            return @string[0].ToString().ToLower() + @string.Replace(".", string.Empty).Substring(1);
        }
    }

}