using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpStackToCSharpCode.Window
{
    public class ChosenLocalsEventArgs : EventArgs
    {
        public ChosenLocalsEventArgs(IEnumerable<string> ckeckedLocals)
        {
            CkeckedLocals = ckeckedLocals;
        }

        public IEnumerable<string> CkeckedLocals { get; }
    }
}
