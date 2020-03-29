using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpStackToCSharpCode.Command.Util
{
    public class ArgumentsListPurifier
    {
        public IReadOnlyList<string> Purify(IReadOnlyList<string> chosenLocals)
        {
            var chosenLocalsProcessed = new List<string>();
            if (chosenLocals == null)
            {
                return chosenLocalsProcessed;
            }

            foreach (var item in chosenLocals)
            {
                var splittedValues = item.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                chosenLocalsProcessed.Add(splittedValues.Last());
            }

            return chosenLocalsProcessed;
        }
    }
}
