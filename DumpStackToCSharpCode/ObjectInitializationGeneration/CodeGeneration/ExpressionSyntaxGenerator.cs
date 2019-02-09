using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ObjectInitializationGeneration.Type;

namespace ObjectInitializationGeneration.CodeGeneration
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
            return SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                SyntaxFactory.IdentifierName(
                    SyntaxFactory.Identifier(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Tab),
                        expressionData.Name,
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Space))), GenerateSyntaxForPrimitiveExpression(expressionData.Type, expressionData.Value));
        }

        public ExpressionSyntax GenerateAssignmentExpressionForComplexExpressionSyntax(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            return SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                SyntaxFactory.IdentifierName(
                    SyntaxFactory.Identifier(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Tab),
                        expressionData.Name,
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Space))), GenerateSyntaxForComplexExpression(expressionData, expressionsSyntax));
        }

        public ExpressionSyntax GenerateSyntaxForPrimitiveExpression(string type, string value)
        {
            if (value == NullValue)
            {
                return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
            }

            switch (type)
            {
                case "short":
                case "ushort":
                case "byte":
                case "sbyte":
                case "int":
                    {
                        return SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(int.Parse(value)));
                    }
                case "float":
                    {
                        return SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(float.Parse(value)));
                    }
                case "double":
                    {
                        return SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(double.Parse(value)));
                    }
                case "decimal":
                    {
                        return SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(decimal.Parse(value)));
                    }
                case "uint":
                    {
                        return SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(uint.Parse(value)));
                    }
                case "long":
                    {
                        return SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(long.Parse(value)));
                    }
                case "ulong":
                    {
                        return SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(ulong.Parse(value)));
                    }
                case "char":
                    {
                        return SyntaxFactory.LiteralExpression(
                            SyntaxKind.CharacterLiteralExpression,
                            SyntaxFactory.Literal(value));
                    }
                case "string":
                    {
                        if (value == NullValue)
                        {
                            return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
                        }

                        return SyntaxFactory.LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            SyntaxFactory.Literal(value.Replace("\"", string.Empty)));
                    }
                case "bool":
                    return SyntaxFactory.LiteralExpression(value == TrueValue ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);

                default:
                    return SyntaxFactory.ObjectCreationExpression(
                               SyntaxFactory.IdentifierName(ParseConcreteType(type)))
                           .WithNewKeyword(
                               SyntaxFactory.Token(
                                   SyntaxFactory.TriviaList(),
                                   SyntaxKind.NewKeyword,
                                   SyntaxFactory.TriviaList(
                                       SyntaxFactory.Space)))
                           .WithArgumentList(
                               SyntaxFactory.ArgumentList()
                                   .WithCloseParenToken(
                                       SyntaxFactory.Token(
                                           SyntaxFactory.TriviaList(),
                                           SyntaxKind.CloseParenToken,
                                           SyntaxFactory.TriviaList())));
            }
        }

        public ExpressionSyntax GenerateSyntaxForComplexExpression(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            if (_typeAnalyzer.IsArray(expressionData.Type))
            {
                return GenerateCreationExpressionForArray(expressionData, expressionsSyntax);
            }

            return GenerateCreationExpressionForComplexType(expressionData, expressionsSyntax);
        }

        private ExpressionSyntax GenerateCreationExpressionForArray(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            return SyntaxFactory.ObjectCreationExpression(
                       SyntaxFactory.IdentifierName(ParseConcreteType(expressionData.Type)))
                   .WithNewKeyword(
                       SyntaxFactory.Token(
                           SyntaxFactory.TriviaList(),
                           SyntaxKind.NewKeyword,
                           SyntaxFactory.TriviaList(
                               SyntaxFactory.Space))).WithInitializer(
                       SyntaxFactory.InitializerExpression(
                           SyntaxKind.ObjectInitializerExpression, expressionsSyntax));
        }

        private ObjectCreationExpressionSyntax GenerateCreationExpressionForComplexType(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            var enterAfterEachElement = new SyntaxTriviaList();
            if (!_typeAnalyzer.IsCollectionOfPrimitiveType(expressionData.Type))
            {
                enterAfterEachElement = SyntaxFactory.TriviaList(SyntaxFactory.LineFeed);
            }

            return SyntaxFactory.ObjectCreationExpression(
                       SyntaxFactory.IdentifierName(ParseConcreteType(expressionData.Type)))
                   .WithNewKeyword(
                       SyntaxFactory.Token(
                           SyntaxFactory.TriviaList(),
                           SyntaxKind.NewKeyword,
                           SyntaxFactory.TriviaList(
                               SyntaxFactory.Space)))
                   .WithArgumentList(
                       SyntaxFactory.ArgumentList()
                           .WithCloseParenToken(
                               SyntaxFactory.Token(
                                   SyntaxFactory.TriviaList(),
                                   SyntaxKind.CloseParenToken,
                                   enterAfterEachElement))).WithInitializer(
                       SyntaxFactory.InitializerExpression(
                           SyntaxKind.ObjectInitializerExpression, expressionsSyntax));
        }

        public string ParseConcreteType(string type)
        {
            if (IsTypeInterface(type))
            {
                var indexOfBracket = type.IndexOf('{');
                var startIndex = indexOfBracket + 1;
                return type.Substring(startIndex, type.Length - startIndex - 1);
            }

            return type;
        }

        public AssignmentExpressionSyntax GenerateAssignmentForDictionary(ExpressionSyntax keyExpressionSyntax, ExpressionSyntax valueExpressionSyntax)
        {
            return SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                                SyntaxFactory.ImplicitElementAccess()
                                                .WithArgumentList(
                                                    SyntaxFactory.BracketedArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                keyExpressionSyntax)))
                                                    .WithOpenBracketToken(
                                                        SyntaxFactory.Token(
                                                            SyntaxFactory.TriviaList(SyntaxFactory.Tab),
                                                            SyntaxKind.OpenBracketToken,
                                                            SyntaxFactory.TriviaList()))
                                                    .WithCloseBracketToken(
                                                        SyntaxFactory.Token(
                                                            SyntaxFactory.TriviaList(),
                                                            SyntaxKind.CloseBracketToken,
                                                            SyntaxFactory.TriviaList(
                                                                SyntaxFactory.Space)))),
                                                valueExpressionSyntax)
                                            .WithOperatorToken(
                                                SyntaxFactory.Token(
                                                    SyntaxFactory.TriviaList(),
                                                    SyntaxKind.EqualsToken,
                                                    SyntaxFactory.TriviaList(
                                                        SyntaxFactory.Space)));
        }
        private static bool IsTypeInterface(string type)
        {
            return type[type.Length - 1] == '}';
        }
    }
}