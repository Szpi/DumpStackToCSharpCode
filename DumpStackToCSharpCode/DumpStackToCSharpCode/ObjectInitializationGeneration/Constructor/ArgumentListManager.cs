using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public ExpressionData GetArgumentList(ExpressionData expressionData)
        {
            if (!GetArgumentNames(expressionData, out var argumentNames))
            {
                return null;
            }

            var matchedArgumentList = expressionData
                       .UnderlyingExpressionData
                       .Where(x => argumentNames
                                  .Any(argumentName =>string.Compare(x.Name,argumentName,StringComparison.OrdinalIgnoreCase) ==0))
                       .ToList();

            return new ExpressionData(string.Empty, string.Empty, string.Empty, matchedArgumentList);
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