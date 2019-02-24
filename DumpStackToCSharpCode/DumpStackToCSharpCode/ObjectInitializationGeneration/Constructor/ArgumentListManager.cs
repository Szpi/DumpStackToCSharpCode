﻿using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Constructor
{
    public class ArgumentListManager
    {
        private readonly ConcreteTypeAnalyzer _concreteTypeAnalyzer;
        private readonly Dictionary<string, IReadOnlyList<string>> _typeToArgumentNames;
        public ArgumentListManager(Dictionary<string, IReadOnlyList<string>> typeToArgumentNames, ConcreteTypeAnalyzer concreteTypeAnalyzer)
        {
            _typeToArgumentNames = typeToArgumentNames;
            _concreteTypeAnalyzer = concreteTypeAnalyzer;
        }

        public ExpressionData GetArgumentList(ExpressionData expressionData, System.Type type)
        {
            if (!GetArgumentNames(expressionData, out var argumentNames))
            {
                if (type == null)
                {
                    return null;
                }

                var constructors = type.GetConstructors();

                if (!constructors.Any())
                {
                    return null;
                }

                Array.Sort(constructors, (info, constructorInfo) => info.GetParameters().Length - constructorInfo.GetParameters().Length);

                _typeToArgumentNames[expressionData.Type] = constructors.Last().GetParameters().Select(x => x.Name).ToList();

                return GetArgumentList(expressionData, type);
            }
            
            var matchedArgumentList = argumentNames
                .Select(x => expressionData
                             .UnderlyingExpressionData
                             .FirstOrDefault(y => string.Compare(y.Name, x, StringComparison.OrdinalIgnoreCase) == 0))
                .Where(x => x != null)
                .ToList();

            return new ExpressionData(string.Empty, string.Empty, string.Empty, matchedArgumentList, string.Empty);
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