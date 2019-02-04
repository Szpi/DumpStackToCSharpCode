namespace RuntimeTestDataCollector.CodeGeneration
{
    public class TypeAnalyzer
    {
        public bool IsCollection(string type)
        {
            return type.StartsWith("System.Collections.Generic");
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