using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using RuntimeTestDataCollector.Window;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;
namespace DumpStackToCSharpCode.Command
{
    class ChooseLocalsCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("ba684a32-934e-4c57-a8e5-f2ab491d485d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;
        private StackDataDumpControl _stackDataDumpControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpStackToCSharpCodeCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ChooseLocalsCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }
        public static ChooseLocalsCommand Instance
        {
            get;
            private set;
        }

        public async void Execute(object sender, EventArgs e)
        {
            try
            {
                if (_stackDataDumpControl == null)
                {
                    await package.JoinableTaskFactory.RunAsync(async () =>
                    {
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        var window = await package.FindToolWindowAsync(typeof(StackDataDump), 0, true, package.DisposalToken);
                        var windowFrame = (IVsWindowFrame)window.Frame;
                        if (windowFrame.IsVisible() != 0)
                        {
                            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
                        }

                        var stackDataDump = window as StackDataDump;
                        _stackDataDumpControl = stackDataDump?.Content as StackDataDumpControl;
                    });
                }
                _stackDataDumpControl.MainTabControl.SelectedIndex = StackDataDumpControl.LocalsTabIndex;
            }
            catch (Exception)
            {
            }
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in DumpStackToCSharpCodeCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            var commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new ChooseLocalsCommand(package, commandService);
        }
    }
}
