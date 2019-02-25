using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization
{
    public class ArrayInitializationGenerator
    {
        private readonly TypeAnalyzer _typeAnalyzer;

        public ArrayInitializationGenerator(TypeAnalyzer typeAnalyzer)
        {
            _typeAnalyzer = typeAnalyzer;
        }

        public ExpressionSyntax Generate(ExpressionData expressionData,
                                         SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax, TypeCode typeCode)
        {
            var enterAfterEachElement = new SyntaxTriviaList();
            if (!_typeAnalyzer.IsPrimitiveType(typeCode))
            {
                enterAfterEachElement = SyntaxFactory.TriviaList(SyntaxFactory.LineFeed);
            }

            return SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName(expressionData.Type))
                                .WithNewKeyword(
                                    SyntaxFactory.Token(
                                        SyntaxFactory.TriviaList(),
                                        SyntaxKind.NewKeyword,
                                        SyntaxFactory.TriviaList(
                                            SyntaxFactory.Space))).WithInitializer(
                                    SyntaxFactory.InitializerExpression(
                                        SyntaxKind.ObjectInitializerExpression, expressionsSyntax));
        }
    }
}