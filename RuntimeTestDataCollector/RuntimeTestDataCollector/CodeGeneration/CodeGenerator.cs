using System.Runtime.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RuntimeTestDataCollector.CodeGeneration
{
    public class CodeGenerator
    {
        private CompilationUnitSyntax _compilationUnitSyntax;
        private string _lastType;
        private SeparatedSyntaxList<ExpressionSyntax> _lastExpressionSyntax;
        public CodeGenerator()
        {
            _compilationUnitSyntax = CompilationUnit();
        }


        public CodeGenerator WithNewObject(string @type)
        {
            _lastType = @type;
            return this;
        }
        
        public void EndMemberDeclaration()
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
                                                                   _lastType)))
                                                       .WithInitializer(
                                                           EqualsValueClause(
                                                               ObjectCreationExpression(
                                                                       IdentifierName(
                                                                           _lastType))
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
                                                                           _lastExpressionSyntax)))))))
                                                                           .WithSemicolonToken(
                                                                               Token(
                                                                                   TriviaList(),
                                                                                   SyntaxKind.SemicolonToken,
                                                                                   TriviaList(
                                                                                       LineFeed))));
          
            _lastExpressionSyntax = new SeparatedSyntaxList<ExpressionSyntax>();
        }
        public string GetStringDump()
        {
            var workspace = new AdhocWorkspace();
            var formattedSyntax = Microsoft.CodeAnalysis.Formatting.Formatter.Format(_compilationUnitSyntax, workspace);
            return formattedSyntax.ToFullString();
        }
        public CodeGenerator WithInitializeExpression(string name, string type, string value)
        {
            _lastExpressionSyntax = _lastExpressionSyntax.Add(AssignmentExpression(
                                                                  SyntaxKind.SimpleAssignmentExpression,
                                                                  IdentifierName(
                                                                      Identifier(
                                                                          TriviaList(
                                                                              Tab),
                                                                          name,
                                                                          TriviaList(
                                                                              Space))), GetValueOfType(type, value)));
            return this;
        }

        private ExpressionSyntax GetValueOfType(string @type, string value)
        {
            if (@type.StartsWith("System.Collections.Generic.List"))
            {
                return ObjectCreationExpression(GenericName(
                                                        Identifier("List"))
                                                    .WithTypeArgumentList(
                                                        TypeArgumentList(
                                                            SingletonSeparatedList<TypeSyntax>(
                                                                PredefinedType(
                                                                    Token(SyntaxKind.IntKeyword))))))
                                                .WithNewKeyword(
                        Token(
                            TriviaList(),
                            SyntaxKind.NewKeyword,
                            TriviaList(
                                Space)))
                    .WithInitializer(
                        InitializerExpression(
                            SyntaxKind.CollectionInitializerExpression,
                            SeparatedList<ExpressionSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(1)),
                                    Token(SyntaxKind.CommaToken),
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(2)),
                                    Token(SyntaxKind.CommaToken),
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(3)),
                                    Token(SyntaxKind.CommaToken),
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(
                                            TriviaList(),
                                            "4",
                                            4,
                                            TriviaList(
                                                Space)))
                                })));
            }

            switch (@type)
            {
                case "int":
                {
                    return LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        Literal(int.Parse(value)));
                }
                case "string":
                {
                    return LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        Literal(value.Substring(1, value.Length - 2)));
                }
                case "bool":
                    return LiteralExpression( value == "true" ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);
                default:
                    return null;
            }
        }

        private string FirstToLowerWithoutComma(string @string)
        {
            return @string[0].ToString().ToLower() + @string.Replace(".", string.Empty).Substring(1);
        }
    }
    
}