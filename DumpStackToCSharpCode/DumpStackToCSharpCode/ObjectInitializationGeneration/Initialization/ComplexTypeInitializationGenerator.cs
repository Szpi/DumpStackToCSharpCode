using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization
{
    public class ComplexTypeInitializationGenerator
    {
        private readonly TypeAnalyzer _typeAnalyzer;

        public ComplexTypeInitializationGenerator(TypeAnalyzer typeAnalyzer)
        {
            _typeAnalyzer = typeAnalyzer;
        }

        public ExpressionSyntax Generate(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            return GenerateCreationExpressionForComplexType(expressionData, expressionsSyntax);
        }
        
        private ObjectCreationExpressionSyntax GenerateCreationExpressionForComplexType(ExpressionData expressionData, SeparatedSyntaxList<ExpressionSyntax> expressionsSyntax)
        {
            var enterAfterEachElement = new SyntaxTriviaList();
            if (!_typeAnalyzer.IsCollectionOfPrimitiveType(expressionData.Type))
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
    }
}