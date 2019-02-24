using RuntimeTestDataCollector.Command;
using RuntimeTestDataCollector.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

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
        private const string ExpandAll = "Expand all";
        private const string CollapseAll = "Collapse all";

        public string ExpandMenuItemHeader { get; set; } = CollapseAll;
        /// <summary>
        /// Initializes a new instance of the <see cref="StackDataDumpControl"/> class.
        /// </summary>
        /// <param name="stackDataDump"></param>

        public StackDataDumpControl(IReadOnlyList<DumpedObjectToCsharpCode> stackDataDump)
        {
            this.InitializeComponent();
            if (string.IsNullOrEmpty(MaxDepth.Text))
            {
                MaxDepth.Text = GeneralOptions.Instance.MaxObjectDepth.ToString();
            }
            AutomaticallyRefresh.IsChecked = GeneralOptions.Instance.AutomaticallyRefresh;
            stackDataDump = new List<DumpedObjectToCsharpCode>()
            {
                new DumpedObjectToCsharpCode("test", "test")
            };
            CreateStackDumpControls(stackDataDump);
        }

        private void ContextMenuLoaded(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as ContextMenu;
            var menuItem = contextMenu.Items[0] as MenuItem;
            //var textBox = menuItem.Template.FindName("ExpandMenuItem", menuItem) as TextBox;

            //textBox.Text = "Some text";
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
            var expander = new Expander()
            {
                Name = headerText,
                Header = headerText,
                Background = CopyEverythingToClipboard.Background,
                FontFamily = CopyEverythingToClipboard.FontFamily,
                Foreground = CopyEverythingToClipboard.Foreground,
                IsExpanded = GeneralOptions.Instance.AutomaticallyExpand,
                BorderBrush = Brushes.Gray,
                Margin = new Thickness(0, 5, 0, 0),
                Content = new TextBox()
                {
                    IsReadOnly = true,
                    Text = textBoxContent,
                    Background = CopyEverythingToClipboard.Background,
                    FontFamily = CopyEverythingToClipboard.FontFamily,
                    Foreground = CopyEverythingToClipboard.Foreground,
                    BorderBrush = null,
                }
            };

            CreateExpanderContextMenu(expander);
            return expander;
        }

        private void CreateExpanderContextMenu(Expander expander)
        {
            var copyMenuItem = new MenuItem()
            {
                Header = "Copy",
                CommandParameter = expander
            };
            copyMenuItem.Click += CopyMenuItem_OnClick;

            var expandMenuItem = new MenuItem()
            {
                Header = GeneralOptions.Instance.AutomaticallyExpand ? CollapseAll : ExpandAll,
                CommandParameter = expander
            };
            expandMenuItem.Click += ExpandMenuItem_OnClick;

            expander.ContextMenu = new ContextMenu()
            {
                Items =
                {
                    copyMenuItem,
                    expandMenuItem
                }
            };
        }

        private void CopyMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
            {
                return;
            }

            if (!(menuItem.CommandParameter is Expander expander))
            {
                return;
            }

            if (expander.Content is TextBox textBox)
            {
                Clipboard.SetText(textBox.Text);
            }
        }

        private void ExpandMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
            {
                return;
            }

            if (!(menuItem.CommandParameter is Expander expander))
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
                menuItem.Header = child.IsExpanded ? CollapseAll : ExpandAll;
            }
        }
    }
}