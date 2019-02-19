using RuntimeTestDataCollector.Command;
using System;
using System.Collections.Generic;
using System.Text;

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
        /// <param name="stackDataDump"></param>
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
        private void CopyEverythingToClipBoard_Click(object sender, RoutedEventArgs e)
        {
            var buffer = new StringBuilder();
            foreach (var child in DumpDataStack.Children)
            {
                var expander = child as Expander;

                if (!(expander?.Content is TextBox textBox))
                {
                    continue;
                }

                buffer.AppendLine(textBox.Text + Environment.NewLine);
            }
            Clipboard.SetText(buffer.ToString());
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
                Background = CopyEverythingToClipboard.Background,
                FontFamily = CopyEverythingToClipboard.FontFamily,
                Foreground = CopyEverythingToClipboard.Foreground,
                Content = new TextBox()
                {
                    IsReadOnly = true,
                    Text = textBoxContent,
                    Background = CopyEverythingToClipboard.Background,
                    FontFamily = CopyEverythingToClipboard.FontFamily,
                    Foreground = CopyEverythingToClipboard.Foreground,
                }
            };
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
            {
                return;
            }

            if (!(menuItem.CommandParameter is ContextMenu contextMenu))
            {
                return;
            }

            if (!(contextMenu.PlacementTarget is Expander expander))
            {
                return;
            }

            var menuItemHeader = menuItem.Header as string;

            if (ExecuteCopyContextItem(menuItemHeader, expander))
            {
                return;
            }

            ExecuteExpandCollapseContextItem(menuItem);
        }

        private void ExecuteExpandCollapseContextItem(MenuItem menuItem)
        {
            foreach (Expander child in DumpDataStack.Children)
            {
                child.IsExpanded = !child.IsExpanded;
                menuItem.Header = child.IsExpanded ? "Expand all" : "Collapse all";
            }
        }

        private static bool ExecuteCopyContextItem(string menuItemHeader, Expander expander)
        {
            if (menuItemHeader == "Copy")
            {
                if (expander.Content is TextBox textBox)
                {
                    Clipboard.SetText(textBox.Text);
                }

                return true;
            }

            return false;
        }
    }
}