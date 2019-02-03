using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RuntimeTestDataCollector.CodeGeneration
{
    public class ExpressionSyntaxGenerator
    {
        private const string NullValue = "null";
        private const string TrueValue = "true";
        private readonly TypeAnalyzer _typeAnalyzer;

        public ExpressionSyntaxGenerator(TypeAnalyzer typeAnalyzer)
        {
            _typeAnalyzer = typeAnalyzer;
        }

        public ExpressionSyntax GenerateAssignmentExpressionForPrimitiveType(ExpressionData expressionData)
        {
            return AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(
                    Identifier(
                        TriviaList(
                            Tab),
                        expressionData.Name,
                        TriviaList(
                            Space))), GenerateSyntaxForPrimitiveExpression(expressionData));
        }

        public ExpressionSyntax GenerateAssignmentExpressionForComplexExpressionSyntax(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            return AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(
                    Identifier(
                        TriviaList(
                            Tab),
                        expressionData.Name,
                        TriviaList(
                            Space))), GenerateSyntaxForComplexExpression(expressionData, expressionsSyntax));
        }

        public ExpressionSyntax GenerateSyntaxForPrimitiveExpression(ExpressionData expressionData)
        {
            switch (expressionData.Type)
            {
                case "short":
                case "ushort":
                case "byte":
                case "sbyte":
                case "int":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(int.Parse(expressionData.Value)));
                    }
                case "float":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(float.Parse(expressionData.Value)));
                    }
                case "double":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(double.Parse(expressionData.Value)));
                    }
                case "decimal":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(decimal.Parse(expressionData.Value)));
                    }
                case "uint":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(uint.Parse(expressionData.Value)));
                    }
                case "long":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(long.Parse(expressionData.Value)));
                    }
                case "ulong":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(ulong.Parse(expressionData.Value)));
                    }
                case "char":
                    {
                        return LiteralExpression(
                            SyntaxKind.CharacterLiteralExpression,
                            Literal(expressionData.Value));
                    }
                case "string":
                    {
                        if (expressionData.Value == NullValue)
                        {
                            return LiteralExpression(SyntaxKind.NullLiteralExpression);
                        }

                        return LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            Literal(expressionData.Value.Replace("\"", string.Empty)));
                    }
                case "bool":
                    return LiteralExpression(expressionData.Value == TrueValue ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);
                default:
                    return ObjectCreationExpression(
                               IdentifierName(expressionData.Type))
                           .WithNewKeyword(
                               Token(
                                   TriviaList(),
                                   SyntaxKind.NewKeyword,
                                   TriviaList(
                                       Space)))
                           .WithArgumentList(
                               ArgumentList()
                                   .WithCloseParenToken(
                                       Token(
                                           TriviaList(),
                                           SyntaxKind.CloseParenToken,
                                           TriviaList())));
            }
        }

        private ExpressionSyntax GenerateSyntaxForComplexExpression(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            if (_typeAnalyzer.IsCollection(expressionData.Type))
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
                            expressionsSyntax));
            }

            return ObjectCreationExpression(
                       IdentifierName(expressionData.Type))
                   .WithNewKeyword(
                       Token(
                           TriviaList(),
                           SyntaxKind.NewKeyword,
                           TriviaList(
                               Space)))
                   .WithArgumentList(
                       ArgumentList()
                           .WithCloseParenToken(
                               Token(
                                   TriviaList(),
                                   SyntaxKind.CloseParenToken,
                                   TriviaList(
                                       LineFeed)))).WithInitializer(
                       InitializerExpression(
                           SyntaxKind.ObjectInitializerExpression, expressionsSyntax));
        }
    }
}