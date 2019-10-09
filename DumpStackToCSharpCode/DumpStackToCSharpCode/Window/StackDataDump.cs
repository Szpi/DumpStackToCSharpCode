using System.Collections.Generic;

namespace RuntimeTestDataCollector.Window
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid(WindowGuidString)]
    public class StackDataDump : ToolWindowPane
    {
        public const string WindowGuidString = "a89bfe9d-7c36-4e87-89be-9e4ca047a865"; // Replace with new GUID in your own code
        public const string Title = "StackDataDump"; // Replace with new GUID in your own code

        /// <summary>
        /// Initializes a new instance of the <see cref="StackDataDump"/> class.
        /// </summary>
        public StackDataDump((IReadOnlyList<DumpedObjectToCsharpCode> dumpedObjectToCsharpCodes, string errorMessage) stackDataDump) : base(null)
        {
            this.Caption = Title;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new StackDataDumpControl(stackDataDump);
        }
    }
}
