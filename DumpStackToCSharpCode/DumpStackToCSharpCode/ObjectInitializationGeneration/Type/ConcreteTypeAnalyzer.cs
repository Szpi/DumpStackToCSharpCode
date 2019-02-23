namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Type
{
    public class ConcreteTypeAnalyzer
    {
        public string ParseConcreteType(string type)
        {
            if (IsTypeInterface(type))
            {
                var indexOfBracket = type.IndexOf('{');
                var startIndex = indexOfBracket + 1;
                return type.Substring(startIndex, type.Length - startIndex - 1);
            }

            return type;
        }

        private static bool IsTypeInterface(string type)
        {
            return type[type.Length - 1] == '}';
        }
    }
}