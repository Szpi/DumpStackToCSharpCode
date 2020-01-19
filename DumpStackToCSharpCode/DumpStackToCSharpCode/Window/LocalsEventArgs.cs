using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpStackToCSharpCode.Window
{
    public class ChosenLocalsEventArgs : EventArgs
    {
        public ChosenLocalsEventArgs(IList<string> ckeckedLocals)
        {
            CkeckedLocals = ckeckedLocals;
        }

        public IList<string> CkeckedLocals { get; }
    }
}
