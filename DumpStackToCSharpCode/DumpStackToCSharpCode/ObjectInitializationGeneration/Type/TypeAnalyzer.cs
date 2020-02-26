using System;
using RuntimeTestDataCollector.ObjectInitializationGeneration.Expression;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Type
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
            var (success, genericType) = GetGenericType(type);

            return success && IsPrimitiveType(genericType, string.Empty);
        }

        public (bool success, string genericType) GetGenericType(string type)
        {
            if (type.Length < 3)
            {
                return (false, null);
            }

            var indexOfBracket = type.IndexOf('<');
            if (indexOfBracket < 0)
            {
                return (false, null);
            }

            var indexOfClosingBracket = type.LastIndexOf('>');
            var startIndex = indexOfBracket + 1;
            var genericType = type.Substring(startIndex, indexOfClosingBracket - startIndex);

            return (true, genericType);
        }

        public TypeCode GetTypeCode(string type, string value)
        {
            if (value == PrimitiveExpressionGenerator.NullValue)
            {
                return TypeCode.NullValue;
            }

            switch (type.TrimEnd('?'))
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
                case "System.Guid":
                    return TypeCode.Guid;
                default:
                    {
                        if (IsArray(type))
                        {
                            return TypeCode.Array;
                        }
                        if (IsDictionary(type))
                        {
                            return TypeCode.Dictionary;
                        }

                        if (IsDictionaryKeyValuePair(type))
                        {
                            return TypeCode.DictionaryKeyValuePair;
                        }

                        if (IsCollection(type))
                        {
                            return TypeCode.Collection;
                        }

                        if (IsEnum(value))
                        {
                            return TypeCode.Enum;
                        }
                        return TypeCode.ComplexObject;
                    }
            }
        }

        private bool IsEnum(string type)
        {
            return !type.StartsWith("{");
        }

        public bool IsPrimitiveType(string type, string value)
        {
            var typeCode = GetTypeCode(type, value);
            return IsPrimitiveType(typeCode);
        }

        public bool IsPrimitiveType(TypeCode typeCode)
        {
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
                case TypeCode.Short:
                case TypeCode.UShort:
                case TypeCode.String:
                case TypeCode.NullValue:
                case TypeCode.Enum:
                    return true;
                default:
                    return false;
            }
        }
    }
}