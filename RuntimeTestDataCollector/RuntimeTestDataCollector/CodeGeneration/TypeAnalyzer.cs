namespace RuntimeTestDataCollector.CodeGeneration
{
    public class TypeAnalyzer
    {
        public bool IsCollection(string type)
        {
            return type.StartsWith("System.Collections.Generic");
        }
    }
}