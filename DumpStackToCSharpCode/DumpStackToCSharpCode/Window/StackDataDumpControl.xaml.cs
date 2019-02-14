using RuntimeTestDataCollector.Command;
using System.Collections.Generic;

namespace RuntimeTestDataCollector.Window
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for StackDataDumpControl.
    /// </summary>
    public partial class StackDataDumpControl : UserControl
    {
        public const int DefaultMaxObjectDepth = 10;
        /// <summary>
        /// Initializes a new instance of the <see cref="StackDataDumpControl"/> class.
        /// </summary>
        /// <param name="stackDataDumpText"></param>
        public StackDataDumpControl(IReadOnlyList<DumpedObjectToCsharpCode> stackDataDump)
        {
            this.InitializeComponent();
            if (string.IsNullOrEmpty(MaxDepth.Text))
            {
                MaxDepth.Text = DefaultMaxObjectDepth.ToString();
            }

            CreateStackDumpControls(stackDataDump);
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void CopyToClipBoard_Click(object sender, RoutedEventArgs e)
        {
            //Clipboard.SetText(StackDumpText.Text);
        }

        private void AutomaticallyRefresh_Checked(object sender, RoutedEventArgs e)
        {
            DumpStackToCSharpCodeCommand.Instance.SubscribeForDebuggerContextChange();
        }

        private void AutomaticallyRefresh_Unchecked(object sender, RoutedEventArgs e)
        {
            DumpStackToCSharpCodeCommand.Instance.UnSubscribeForDebuggerContextChange();
        }

        public void CreateStackDumpControls(IReadOnlyList<DumpedObjectToCsharpCode> stackDataDump)
        {
            DumpDataStack.Children.Clear();

            foreach (var dumpedObjectToCsharpCode in stackDataDump)
            {
                var expander = CreateExpander(dumpedObjectToCsharpCode.Name, dumpedObjectToCsharpCode.CsharpCode);
                DumpDataStack.Children.Add(expander);
            }
        }

        private Expander CreateExpander(string headerText, string textBoxContent)
        {
            return new Expander()
            {
                Name = headerText,
                Header = headerText,
                Background = CopyToClipboard.Background,
                FontFamily = CopyToClipboard.FontFamily,
                Foreground = CopyToClipboard.Foreground,
                Content = new TextBox()
                {
                    Text = textBoxContent,
                    Background = CopyToClipboard.Background,
                    FontFamily = CopyToClipboard.FontFamily,
                    Foreground = CopyToClipboard.Foreground,
                }
            };
        }
    }
}