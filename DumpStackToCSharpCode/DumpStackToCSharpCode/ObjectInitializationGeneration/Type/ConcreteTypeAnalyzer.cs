using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
            var tokens = Regex.Split(type, @"(?<=[<,>])").Where(x => !string.IsNullOrEmpty(x));
            var buffer = new StringBuilder();
            foreach (var token in tokens)
            {
                var tokenType = ParseConcreteType(RemoveNamespace(token));
                buffer.Append(tokenType);
            }

            return buffer.ToString();
        }

        private string RemoveNamespace(string stringToRemoveNamespace)
        {
            var dotIndex = stringToRemoveNamespace.LastIndexOf('.');
            if (dotIndex <= 0)
            {
                return stringToRemoveNamespace;
            }

            return stringToRemoveNamespace.Substring(dotIndex + 1);
        }
       
        private static bool IsTypeInterface(string type)
        {
            return type[type.Length - 1] == '}';
        }
    }
}