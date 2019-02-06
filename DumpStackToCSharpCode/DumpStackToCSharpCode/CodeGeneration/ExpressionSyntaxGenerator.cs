using System.Linq;
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
                            Space))), GenerateSyntaxForPrimitiveExpression(expressionData.Type, expressionData.Value));
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

        public ExpressionSyntax GenerateSyntaxForPrimitiveExpression(string type, string value)
        {
            if (value == NullValue)
            {
                return LiteralExpression(SyntaxKind.NullLiteralExpression);
            }

            switch (type)
            {
                case "short":
                case "ushort":
                case "byte":
                case "sbyte":
                case "int":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(int.Parse(value)));
                    }
                case "float":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(float.Parse(value)));
                    }
                case "double":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(double.Parse(value)));
                    }
                case "decimal":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(decimal.Parse(value)));
                    }
                case "uint":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(uint.Parse(value)));
                    }
                case "long":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(long.Parse(value)));
                    }
                case "ulong":
                    {
                        return LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(ulong.Parse(value)));
                    }
                case "char":
                    {
                        return LiteralExpression(
                            SyntaxKind.CharacterLiteralExpression,
                            Literal(value));
                    }
                case "string":
                    {
                        if (value == NullValue)
                        {
                            return LiteralExpression(SyntaxKind.NullLiteralExpression);
                        }

                        return LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            Literal(value.Replace("\"", string.Empty)));
                    }
                case "bool":
                    return LiteralExpression(value == TrueValue ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);

                default:
                    return ObjectCreationExpression(
                               IdentifierName(ParseConcreteType(type)))
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
            if (_typeAnalyzer.IsArray(expressionData.Type))
            {
                return GenerateCreationExpressionForArray(expressionData, expressionsSyntax);
            }

            return GenerateCreationExpressionForComplexType(expressionData, expressionsSyntax);
        }

        private ExpressionSyntax GenerateCreationExpressionForArray(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            return ObjectCreationExpression(
                       IdentifierName(ParseConcreteType(expressionData.Type)))
                   .WithNewKeyword(
                       Token(
                           TriviaList(),
                           SyntaxKind.NewKeyword,
                           TriviaList(
                               Space))).WithInitializer(
                       InitializerExpression(
                           SyntaxKind.ObjectInitializerExpression, expressionsSyntax));
        }

        private ObjectCreationExpressionSyntax GenerateCreationExpressionForComplexType(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            var enterAfterEachElement = new SyntaxTriviaList();
            if (!_typeAnalyzer.IsCollectionOfPrimitiveType(expressionData.Type))
            {
                enterAfterEachElement = TriviaList(LineFeed);
            }

            return ObjectCreationExpression(
                       IdentifierName(ParseConcreteType(expressionData.Type)))
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
                                   enterAfterEachElement))).WithInitializer(
                       InitializerExpression(
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

        public AssignmentExpressionSyntax GenerateAssignmentForDictionary(ExpressionData expression)
        {
            var dictionaryKey = expression.UnderlyingExpressionData.First(x => x.Name == "Key");
            var dictionaryValue = expression.UnderlyingExpressionData.First(x => x.Name == "Value");

            var keyValueExpression = GenerateSyntaxForPrimitiveExpression(dictionaryKey.Type, dictionaryKey.Value);
            var valueValueExpression = GenerateSyntaxForPrimitiveExpression(dictionaryValue.Type, dictionaryValue.Value);

            return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                                ImplicitElementAccess()
                                                .WithArgumentList(
                                                    BracketedArgumentList(
                                                        SingletonSeparatedList<ArgumentSyntax>(
                                                            Argument(
                                                                keyValueExpression)))
                                                    .WithOpenBracketToken(
                                                        Token(
                                                            TriviaList(Tab),
                                                            SyntaxKind.OpenBracketToken,
                                                            TriviaList()))
                                                    .WithCloseBracketToken(
                                                        Token(
                                                            TriviaList(),
                                                            SyntaxKind.CloseBracketToken,
                                                            TriviaList(
                                                                Space)))),
                                                valueValueExpression)
                                            .WithOperatorToken(
                                                Token(
                                                    TriviaList(),
                                                    SyntaxKind.EqualsToken,
                                                    TriviaList(
                                                        Space)));
        }
        private static bool IsTypeInterface(string type)
        {
            return type[type.Length - 1] == '}';
        }
    }
}