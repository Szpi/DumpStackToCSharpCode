using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using RuntimeTestDataCollector.StackFrameAnalyzer;
using RuntimeTestDataCollector.Window;
using System;
using System.ComponentModel.Design;
using RuntimeTestDataCollector.Options;
using Task = System.Threading.Tasks.Task;

namespace RuntimeTestDataCollector.Command
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class DumpStackToCSharpCodeCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("546abd90-d54f-42c1-a8ac-26fdd0f6447d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        private static DTE2 _dte;
        private static DebuggerEvents _debuggerEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpStackToCSharpCodeCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private DumpStackToCSharpCodeCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static DumpStackToCSharpCodeCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider => this.package;
        private StackDataDumpControl _stackDataDumpControl;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in DumpStackToCSharpCodeCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            var dte = await package.GetServiceAsync(typeof(DTE)) ?? throw new Exception("GetServiceAsync returned DTE null");

            _dte = dte as DTE2;
            _debuggerEvents = _dte.Events.DebuggerEvents;

            var commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new DumpStackToCSharpCodeCommand(package, commandService);
        }


        public void SubscribeForDebuggerContextChange()
        {
            _debuggerEvents.OnContextChanged += OnDebuggerContextChange;
        }

        public void UnSubscribeForDebuggerContextChange()
        {
            _debuggerEvents.OnContextChanged -= OnDebuggerContextChange;
        }

        public async Task OnSettingsSaveAsync()
        {
            var generalOptions = await GeneralOptions.GetLiveInstanceAsync();
            if (_stackDataDumpControl == null)
            {
                var window = await package.FindToolWindowAsync(typeof(StackDataDump), 0, true, package.DisposalToken);
                var stackDataDump = window as StackDataDump;
                _stackDataDumpControl = stackDataDump?.Content as StackDataDumpControl;
            }

            _stackDataDumpControl.MaxDepth.Text = generalOptions.MaxObjectDepth.ToString();
            _stackDataDumpControl.AutomaticallyRefresh.IsChecked = generalOptions.AutomaticallyRefresh;
        }

        private void OnDebuggerContextChange(Process newprocess, Program newprogram, Thread newthread, StackFrame newstackframe)
        {
            DumpStackToCSharpCode();
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>


        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            DumpStackToCSharpCode();
        }

        private void DumpStackToCSharpCode()
        {
            if (_stackDataDumpControl == null)
            {
                package.JoinableTaskFactory.RunAsync(async () =>
                {
                    var window = await package.FindToolWindowAsync(typeof(StackDataDump), 0, true, package.DisposalToken);
                    var windowFrame = (IVsWindowFrame)window.Frame;
                    Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());

                    var stackDataDump = window as StackDataDump;
                    _stackDataDumpControl = stackDataDump?.Content as StackDataDumpControl;
                    DumpStackToCSharpCode();
                });
                return;
            }

            var debuggerStackToDumpedObject = new DebuggerStackToDumpedObject();
            var dumpedObjectsToCsharpCode = debuggerStackToDumpedObject.DumpObjectOnStack(_dte, int.Parse(_stackDataDumpControl.MaxDepth.Text), GeneralOptions.Instance.GenerateTypeWithNamespace);

            _stackDataDumpControl.CreateStackDumpControls(dumpedObjectsToCsharpCode);
        }
    }
}
