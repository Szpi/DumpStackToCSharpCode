using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using RuntimeTestDataCollector.CodeGeneration;
using RuntimeTestDataCollector.Window;
using System;
using System.ComponentModel.Design;
using RuntimeTestDataCollector.CodeGeneration.Factory;
using Task = System.Threading.Tasks.Task;

namespace RuntimeTestDataCollector.Command
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GenerateTestDataCommand
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
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateTestDataCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private GenerateTestDataCommand(AsyncPackage package, OleMenuCommandService commandService)
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
        public static GenerateTestDataCommand Instance
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
            // Switch to the main thread - the call to AddCommand in GenerateTestDataCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            _dte = await package.GetServiceAsync(typeof(DTE)) as DTE2;

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new GenerateTestDataCommand(package, commandService);
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

            if (_stackDataDumpControl != null)
            {
                var currentExpressionData = new DebuggerStackFrameAnalyzer().AnalyzeCurrentStack(_dte);

                var codeGeneratorManager = CodeGeneratorManagerFactory.Create();
                _stackDataDumpControl.StackDumpText.Text = codeGeneratorManager.GenerateStackDump(currentExpressionData);
                return;
            }

            package.JoinableTaskFactory.RunAsync(async () =>
            {
                ToolWindowPane window = await package.ShowToolWindowAsync(
                    typeof(StackDataDump),
                    0,
                    create: true,
                    cancellationToken: package.DisposalToken);
                var stackDataDump = window as StackDataDump;
                _stackDataDumpControl = stackDataDump?.Content as StackDataDumpControl;
            });
        }
    }
}
