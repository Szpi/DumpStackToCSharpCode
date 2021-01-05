using System;
using System.Collections.Generic;
using System.Linq;

namespace DumpStackToCSharpCode.ObjectInitializationGeneration.Constructor
{
    public class ConstructorsManager
    {
        public IReadOnlyList<string> GetMostDescriptiveConstructor(System.Type type)
        {
            var constructors = type.GetConstructors();

            if (!constructors.Any())
            {
                return new List<string>();
            }

            Array.Sort(constructors,
                       (info, constructorInfo) => info.GetParameters().Length - constructorInfo.GetParameters().Length);

            return constructors.Last().GetParameters().Select(x => x.Name).ToList();
        }
    }
}
