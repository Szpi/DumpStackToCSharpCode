using System.Collections.Generic;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration
{
    public class ExpressionData
    {

        public string Type { get; }
        public string Value { get; }
        public string Name { get; }
        public string TypeWithNamespace { get; }

        public IReadOnlyList<ExpressionData> UnderlyingExpressionData { get; }

        public ExpressionData(string type,
                              string value,
                              string name,
                              IReadOnlyList<ExpressionData> underlyingExpressionData,
                              string typeWithNamespace)
        {
            Type = type;
            Value = value;
            Name = name;
            UnderlyingExpressionData = underlyingExpressionData;
            TypeWithNamespace = typeWithNamespace;
        }
    }
}