using System;
using System.Globalization;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeCode = RuntimeTestDataCollector.ObjectInitializationGeneration.Type.TypeCode;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Expression
{
    public class PrimitiveExpressionGenerator
    {
        private const string NullValue = "null";
        private const string TrueValue = "true";

        public ExpressionSyntax Generate(TypeCode type, string value)
        {
            if (value == NullValue)
            {
                return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
            }

            switch (type)
            {
                case TypeCode.Short:
                case TypeCode.UShort:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int:
                {
                    return SyntaxFactory.LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        SyntaxFactory.Literal(int.Parse(value, Thread.CurrentThread.CurrentUICulture)));
                }
                case TypeCode.Float:
                {
                    return SyntaxFactory.LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        SyntaxFactory.Literal(float.Parse(value, Thread.CurrentThread.CurrentUICulture)));
                }
                case TypeCode.Double:
                {
                    return SyntaxFactory.LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        SyntaxFactory.Literal(double.Parse(value, Thread.CurrentThread.CurrentUICulture)));
                }
                case TypeCode.Decimal:
                {
                    return SyntaxFactory.LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        SyntaxFactory.Literal(decimal.Parse(value, Thread.CurrentThread.CurrentUICulture)));
                }
                case TypeCode.UInt:
                {
                    return SyntaxFactory.LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        SyntaxFactory.Literal(uint.Parse(value, Thread.CurrentThread.CurrentUICulture)));
                }
                case TypeCode.Long:
                {
                    return SyntaxFactory.LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        SyntaxFactory.Literal(long.Parse(value, Thread.CurrentThread.CurrentUICulture)));
                }
                case TypeCode.ULong:
                {
                    return SyntaxFactory.LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        SyntaxFactory.Literal(ulong.Parse(value, Thread.CurrentThread.CurrentUICulture)));
                }
                case TypeCode.Char:
                {
                    return SyntaxFactory.LiteralExpression(
                        SyntaxKind.CharacterLiteralExpression,
                        SyntaxFactory.Literal(value));
                }
                case TypeCode.String:
                {
                    return SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(value.Replace("\"", string.Empty)));
                }
                case TypeCode.Boolean:
                {
                    return SyntaxFactory.LiteralExpression(value == TrueValue
                                                               ? SyntaxKind.TrueLiteralExpression
                                                               : SyntaxKind.FalseLiteralExpression);
                }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}