using System.ComponentModel;
using DumpStackToCSharpCode.Options;
using Microsoft.VisualStudio.Shell;
using RuntimeTestDataCollector.Command;
using RuntimeTestDataCollector.Window;

namespace RuntimeTestDataCollector.Options
{
    /// <summary>
    /// A provider for custom <see cref="DialogPage" /> implementations.
    /// </summary>
    internal class DialogPageProvider
    {
        public class General : BaseOptionPage<GeneralOptions>
        {
        }

        public class ReadOnlyObjectArgumentsPageProvider : BaseOptionPage<ReadOnlyObjectArgumentsOptions>
        {            
        }
    }
}
