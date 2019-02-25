namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Type
{
    public class ConcreteTypeAnalyzer
    {
        private readonly TypeAnalyzer _typeAnalyzer;

        public ConcreteTypeAnalyzer(TypeAnalyzer typeAnalyzer)
        {
            _typeAnalyzer = typeAnalyzer;
        }

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
            if (_typeAnalyzer.IsCollection(type))
            {
                return ParseConcreteType(GetForGenericType(type));
            }

            var dotIndex = type.LastIndexOf('.');

            return dotIndex < 0 ? type : ParseConcreteType(type.Substring(dotIndex + 1));
        }

        private string GetForGenericType(string type)
        {
            var mainType = type.Substring(0, type.IndexOf('<'));
            var dotIndex = mainType.LastIndexOf('.');
            var (success, genericType) = _typeAnalyzer.GetGenericType(type);
            if (!success)
            {
                return string.Empty;
            }

            var genericTypeWithoutNameSpace = GetTypeWithoutNamespace(genericType);
            return dotIndex < 0 ? type : mainType.Substring(dotIndex + 1) + "<" + genericTypeWithoutNameSpace + ">";
        }

        private static bool IsTypeInterface(string type)
        {
            return type[type.Length - 1] == '}';
        }
    }
}