﻿using System.Collections.Generic;

namespace ObjectInitializationGeneration.CodeGeneration
{
    public class ExpressionData
    {
        public string Type { get; }
        public string Value { get; }
        public string Name { get; }
        public IReadOnlyList<ExpressionData> UnderlyingExpressionData { get; }

        public ExpressionData(string type, string value, string name, IReadOnlyList<ExpressionData> underlyingExpressionData)
        {
            Type = type;
            Value = value;
            Name = name;
            UnderlyingExpressionData = underlyingExpressionData;
        }

    }
}