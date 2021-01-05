using DumpStackToCSharpCode.Command.Util;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

            var builtInTypes = new Dictionary<string, IReadOnlyList<string>>
            {
                [nameof(Uri)] = new List<string> { nameof(Uri.AbsoluteUri) },
            };

            if (_typeToArgumentNames != null)
            {
                _typeToArgumentNames = _typeToArgumentNames.Concat(builtInTypes.Where(x => !_typeToArgumentNames.ContainsKey(x.Key)))
                    .ToDictionary(x => x.Key, x => x.Value);
            }
            else
            {
                _typeToArgumentNames = builtInTypes;
            }
        }

        public ExpressionData GetArgumentList(ExpressionData expressionData, System.Type type)
        {
            if (!GetArgumentNames(expressionData, out var argumentNames))
            {
                return TryAddConstructorArguments(expressionData, type) ? null : GetArgumentList(expressionData, type);
            }

            var matchedArgumentList = argumentNames
                .Select(x => expressionData
                             .UnderlyingExpressionData
                             .FirstOrDefault(y => string.Compare(y.Name, x, StringComparison.OrdinalIgnoreCase) == 0))
                .Where(x => x != null)
                .ToList();

            return new ExpressionData(string.Empty, string.Empty, string.Empty, matchedArgumentList, string.Empty);
        }

        private bool TryAddConstructorArguments(ExpressionData expressionData, System.Type type)
        {
            if (type == null)
            {
                return true;
            }

            var constructors = type.GetConstructors();

            if (!constructors.Any())
            {
                return true;
            }

            Array.Sort(constructors,
                       (info, constructorInfo) => info.GetParameters().Length - constructorInfo.GetParameters().Length);

            _typeToArgumentNames[expressionData.Type] = constructors.Last().GetParameters().Select(x => x.Name).ToList();
            return false;
        }

        private bool GetArgumentNames(ExpressionData expressionData, out IReadOnlyList<string> argumentNames)
        {
            if (_typeToArgumentNames == null)
            {
                argumentNames = new List<string>();
                return false;
            }

            if (_typeToArgumentNames.TryGetValue(expressionData.Type.TrimEnd('?'), out argumentNames))
            {
                return true;
            }

            var typeWithoutNamespace = _concreteTypeAnalyzer.GetTypeWithoutNamespace(expressionData.Type);
            return _typeToArgumentNames.TryGetValue(typeWithoutNamespace, out argumentNames);
        }
    }
}