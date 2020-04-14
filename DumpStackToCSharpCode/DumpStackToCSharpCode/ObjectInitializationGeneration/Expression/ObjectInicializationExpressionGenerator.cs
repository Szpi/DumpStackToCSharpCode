using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DumpStackToCSharpCode.ObjectInitializationGeneration.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.Expression
{
    public class ObjectInicializationExpressionGenerator
    {
        private readonly TypeAnalyzer _typeAnalyzer;

        public ObjectInicializationExpressionGenerator(TypeAnalyzer typeAnalyzer)
        {
            _typeAnalyzer = typeAnalyzer;
        }

        public ExpressionSyntax GenerateForArray(string type, SeparatedSyntaxList<ExpressionSyntax> expressions)
        {
            return SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(type))
                                                                       .WithInitializer(SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression, expressions));
        }

        public ExpressionSyntax GenerateForObject(string type,
                                     SeparatedSyntaxList<ExpressionSyntax> expressions,
                                     List<ExpressionSyntax> argumentSyntax)
        {
            var argumentList = GenerateArgumentListSyntaxWithCommas(argumentSyntax);
            InitializerExpressionSyntax initializerExpressionSyntax = null;

            if (!_typeAnalyzer.IsArray(type) && argumentList.Arguments.Count == 0)
            {
                argumentList = CreateEmptyArgumentList();

                initializerExpressionSyntax =
                    SyntaxFactory.InitializerExpression(
                        SyntaxKind
                            .ObjectInitializerExpression,
                        expressions);
            }

            return SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(
                                                                           type))
                                                                   .WithArgumentList(argumentList)
                                                                   .WithInitializer(initializerExpressionSyntax);
        }
        private static ArgumentListSyntax GenerateArgumentListSyntaxWithCommas(List<ExpressionSyntax> argumentSyntax)
        {
            var argumentListSyntaxWithCommas = new List<SyntaxNodeOrToken>();
            var allArgumentsLengthRequiresBreak = argumentSyntax?.Sum(x => x.FullSpan.Length) > 50 && argumentSyntax?.Count > 2;
            for (var i = 0; i < argumentSyntax?.Count; i++)
            {
                argumentListSyntaxWithCommas.Add(SyntaxFactory.Argument(argumentSyntax[i]));

                if (i == argumentSyntax.Count - 1)
                {
                    continue;
                }

                if (allArgumentsLengthRequiresBreak)
                {
                    argumentListSyntaxWithCommas.Add(CreateCommaTokenWithEnter());
                }
                else
                {
                    argumentListSyntaxWithCommas.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                }
            }

            return SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(argumentListSyntaxWithCommas));
        }

        private static SyntaxToken CreateCommaTokenWithEnter()
        {
            return SyntaxFactory.Token(SyntaxFactory.TriviaList(),
                                       SyntaxKind.CommaToken,
                                       SyntaxFactory.TriviaList(
                                       SyntaxFactory.LineFeed));
        }

        private static ArgumentListSyntax CreateEmptyArgumentList()
        {
            return SyntaxFactory.ArgumentList().WithCloseParenToken(
                SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.CloseParenToken,
                    SyntaxFactory.TriviaList(
                        SyntaxFactory.LineFeed)));
        }
    }
}
