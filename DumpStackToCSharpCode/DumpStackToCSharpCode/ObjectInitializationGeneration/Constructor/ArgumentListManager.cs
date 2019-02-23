using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Initialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Constructor
{
    public class ArgumentListManager
    {
        private readonly ConcreteTypeAnalyzer _concreteTypeAnalyzer;
        private readonly IReadOnlyDictionary<string, IReadOnlyList<string>> _typeToArgumentNames;
        public ArgumentListManager(IReadOnlyDictionary<string, IReadOnlyList<string>> typeToArgumentNames, ConcreteTypeAnalyzer concreteTypeAnalyzer)
        {
            _typeToArgumentNames = typeToArgumentNames;
            _concreteTypeAnalyzer = concreteTypeAnalyzer;
        }

        public List<ExpressionSyntax> GetArgumentList(ExpressionData expressionData, InitializationManager initializationManager)
        {
            if (!GetArgumentNames(expressionData, out var argumentNames))
            {
                return new List<ExpressionSyntax>();
            }

            var argumentList = new List<ExpressionSyntax>();

            foreach (var argumentName in argumentNames)
            {
                var argument = expressionData.UnderlyingExpressionData.FirstOrDefault(x => string.Compare(x.Name, argumentName, StringComparison.OrdinalIgnoreCase) == 0);

                if (argument == null)
                {
                    continue;
                }

                var (generatedSyntax, _, _) = initializationManager.Generate(argument);

                var first = generatedSyntax.FirstOrDefault();
                if (first is AssignmentExpressionSyntax assignmentExpression)
                {
                    argumentList.Add(assignmentExpression.Right);
                    continue;
                }

                argumentList.Add(first);
            }

            return argumentList;
        }

        private bool GetArgumentNames(ExpressionData expressionData, out IReadOnlyList<string> argumentNames)
        {
            if (_typeToArgumentNames.TryGetValue(expressionData.Type, out argumentNames))
            {
                return true;
            }

            var typeWithoutNamespace = _concreteTypeAnalyzer.GetTypeWithoutNamespace(expressionData.Type);
            return _typeToArgumentNames.TryGetValue(typeWithoutNamespace, out argumentNames);
        }
    }
}