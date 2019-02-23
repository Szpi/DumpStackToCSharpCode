namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Type
{
    public class ConcreteTypeAnalyzer
    {
        public string ParseConcreteType(string type)
        {
            if (!IsTypeInterface(type))
            {
                return type;
            }

            var indexOfBracket = type.IndexOf('{');
            var startIndex = indexOfBracket + 1;
            return type.Substring(startIndex, type.Length - startIndex - 1);
        }

        public string GetTypeWithoutNamespace(string type)
        {
            var dotIndex = type.LastIndexOf('.');
            return dotIndex < 0 ? type : type.Substring(dotIndex + 1);
        }

        private static bool IsTypeInterface(string type)
        {
            return type[type.Length - 1] == '}';
        }
    }
}