namespace RuntimeTestDataCollector.CodeGeneration
{
    public class TypeAnalyzer
    {
        public bool IsCollection(string type)
        {
            return type.StartsWith("System.Collections.Generic");
        }

        public bool IsArray(string type)
        {
            return type.Length > 2 && type[type.Length - 1] == ']' && type[type.Length - 2] == '[';
        }

        public bool IsCollectionOfPrimitiveType(string type)
        {
            if (type.Length < 3)
            {
                return false;
            }

            var indexOfBracket = type.IndexOf('<');
            if (indexOfBracket < 0)
            {
                return false;
            }

            var indexOfClosingBracket = type.IndexOf('>');
            var startIndex = indexOfBracket + 1;
            return IsPrimitiveType(type.Substring(startIndex, indexOfClosingBracket - startIndex));
        }

        public bool IsPrimitiveType(string type)
        {
            switch (type)
            {
                case "bool":
                case "byte":
                case "sbyte":
                case "char":
                case "decimal":
                case "double":
                case "float":
                case "int":
                case "uint":
                case "long":
                case "ulong":
                case "object":
                case "short":
                case "ushort":
                case "string":
                    return true;
                default:
                    return false;
            }
        }
    }
}