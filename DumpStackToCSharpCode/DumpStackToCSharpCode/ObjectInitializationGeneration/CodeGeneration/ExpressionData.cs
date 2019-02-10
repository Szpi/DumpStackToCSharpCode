using System.Collections.Generic;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Type;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration
{
    public class ExpressionData
    {
        private static readonly ConcreteTypeAnalyzer ConcreteTypeAnalyzer = new ConcreteTypeAnalyzer();

        public string Type { get; }
        public string Value { get; }
        public string Name { get; }
        public IReadOnlyList<ExpressionData> UnderlyingExpressionData { get; }

        public ExpressionData(string type, string value, string name, IReadOnlyList<ExpressionData> underlyingExpressionData)
        {
            Type = ConcreteTypeAnalyzer.ParseConcreteType(type);
            Value = value;
            Name = name;
            UnderlyingExpressionData = underlyingExpressionData;
        }
    }
}