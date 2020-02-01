using System;
using System.Collections.Generic;

namespace RuntimeTestDataCollector.ObjectInitializationGeneration.Constructor
{
    public class OrdinalIgnoreCaseComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}