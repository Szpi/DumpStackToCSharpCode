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
           public override void SaveSettingsToStorage()
            {
                base.SaveSettingsToStorage();
                DumpStackToCSharpCodeCommand.Instance.OnSettingsSave();
            }
        }
    }
}
