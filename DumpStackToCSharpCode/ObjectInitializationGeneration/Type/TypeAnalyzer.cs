namespace ObjectInitializationGeneration.Type
{
    public class TypeAnalyzer
    {
        public bool IsCollection(string type)
        {
            return type.StartsWith("System.Collections.Generic");
        }

        public bool IsDictionaryKeyValuePair(string type)
        {
            return type.StartsWith("System.Collections.Generic.KeyValuePair");
        }

        private bool IsDictionary(string type)
        {
            return type.StartsWith("System.Collections.Generic.Dictionary");
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

        public TypeCode GetTypeCode(string type)
        {
            switch (type)
            {
                case "bool":
                    return TypeCode.Boolean;
                case "byte":
                    return TypeCode.Byte;
                case "sbyte":
                    return TypeCode.SByte;
                case "char":
                    return TypeCode.Char;
                case "decimal":
                    return TypeCode.Decimal;
                case "double":
                    return TypeCode.Double;
                case "float":
                    return TypeCode.Float;
                case "int":
                    return TypeCode.Int;
                case "uint":
                    return TypeCode.UInt;
                case "long":
                    return TypeCode.Long;
                case "ulong":
                    return TypeCode.ULong;
                case "object":
                    return TypeCode.Object;
                case "short":
                    return TypeCode.Short;
                case "ushort":
                    return TypeCode.UShort;
                case "string":
                    return TypeCode.String;
                default:
                {
                    if (IsDictionary(type))
                    {
                        return TypeCode.Dictionary;
                    }

                    if (IsCollection(type))
                    {
                        return TypeCode.Collection;
                    }

                    return TypeCode.ComplexObject;
                }
            }
        }

        public bool IsPrimitiveType(string type)
        {
            var typeCode = GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Char:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Float:
                case TypeCode.Int:
                case TypeCode.UInt:
                case TypeCode.Long:
                case TypeCode.ULong:
                case TypeCode.Object:
                case TypeCode.Short:
                case TypeCode.UShort:
                case TypeCode.String:
                    return true;
                default:
                    return false;
            }
        }
    }
}