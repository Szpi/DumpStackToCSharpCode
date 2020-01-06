using RuntimeTestDataCollector.Command;
using RuntimeTestDataCollector.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace RuntimeTestDataCollector.Window
{
    using DumpStackToCSharpCode.CurrentStack;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="StackDataDumpControl"/> class.
        /// </summary>
        /// <param name="stackDataDump"></param>

        public StackDataDumpControl()
        {
            this.InitializeComponent();
            if (string.IsNullOrEmpty(MaxDepth.Text))
            {
                MaxDepth.Text = GeneralOptions.Instance.MaxObjectDepth.ToString();
            }
            AutomaticallyRefresh.IsChecked = GeneralOptions.Instance.AutomaticallyRefresh;
            BusyLabel.Visibility = Visibility.Hidden;
            ErrorMessageRow.Height = new GridLength(0);

            if (GeneralOptions.Instance.ClearControlsOnStart)
            {
                DumpDataStack.Children.Clear();
            }
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

                buffer.AppendLine(textBox.Text);
            }
            Clipboard.SetText(buffer.ToString());
        }

        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void GenerateLocals_Click(object sender, RoutedEventArgs e)
        {
            DumpStackToCSharpCodeCommand.Instance.Execute(this, null);
        }

        private void AutomaticallyRefresh_Checked(object sender, RoutedEventArgs e)
        {
            DumpStackToCSharpCodeCommand.Instance.SubscribeForDebuggerContextChange();
        }

        private void AutomaticallyRefresh_Unchecked(object sender, RoutedEventArgs e)
        {
            DumpStackToCSharpCodeCommand.Instance.UnSubscribeForDebuggerContextChange();
        }

        public void ClearControls()
        {
            DumpDataStack.Children.Clear();
            BusyLabel.Visibility = Visibility.Visible;
            ErrorMessageRow.Height = new GridLength(0);
        }

        public void ResetControls()
        {
            DumpDataStack.Children.Clear();
            BusyLabel.Visibility = Visibility.Hidden;
            ErrorMessageRow.Height = new GridLength(0);
        }


        void OnTabChange(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (!LocalsTab.IsSelected)
            {
                return;
            }

            var locals = DumpStackToCSharpCodeCommand.Instance.GetCurrentStack();

            CreateLocals(locals);
        }
        private void CreateLocals(IReadOnlyCollection<CurrentExpressionOnStack> locals)
        {
            foreach (var local in locals)
            {
                var localVariable = new CheckBox()
                {
                    Content = local.Name,
                    IsChecked = false,
                    Background = AutomaticallyRefresh.Background,
                    FontFamily = AutomaticallyRefresh.FontFamily,
                    Foreground = AutomaticallyRefresh.Foreground,                    
                };

                LocalsStack.Children.Add(localVariable);
            }
        }

        public void CreateStackDumpControls(IReadOnlyList<DumpedObjectToCsharpCode> dumpedObjectsToCsharpCode, string errorMessage)
        {
            BusyLabel.Visibility = Visibility.Hidden;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorMessage.Text = "Error: " + errorMessage;
                ErrorMessageRow.Height = new GridLength(1, GridUnitType.Auto);
            }

            foreach (var dumpedObjectToCsharpCode in dumpedObjectsToCsharpCode)
            {
                var expander = CreateExpander(
                                dumpedObjectToCsharpCode.Name,
                                dumpedObjectToCsharpCode.CsharpCode,
                                !string.IsNullOrEmpty(dumpedObjectToCsharpCode.ErrorMessage));

                DumpDataStack.Children.Add(expander);
            }
        }

        private Expander CreateExpander(string headerText, string textBoxContent, bool causedError)
        {
            var expander = new Expander()
            {
                Name = headerText,
                Header = headerText,
                Background = CopyEverythingToClipboard.Background,
                FontFamily = CopyEverythingToClipboard.FontFamily,
                Foreground = CopyEverythingToClipboard.Foreground,
                IsExpanded = GeneralOptions.Instance.AutomaticallyExpand,
                BorderBrush = causedError ? Brushes.Red : Brushes.Gray,
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

            var copyAllMenuItem = new MenuItem()
            {
                Header = "Copy all",
                CommandParameter = expander
            };
            copyAllMenuItem.Click += CopyEverythingToClipBoard_Click;

            var expandMenuItem = new MenuItem()
            {
                Header = GeneralOptions.Instance.AutomaticallyExpand ? CollapseAll : ExpandAll,
                CommandParameter = expander
            };
            expandMenuItem.Click += ExpandMenuItem_OnClick;

            var clearAllMenuItem = new MenuItem()
            {
                Header = "Clear all",
                CommandParameter = expander
            };
            clearAllMenuItem.Click += ClearAllItem_OnClick;

            expander.ContextMenu = new ContextMenu()
            {
                Items =
                {
                    copyMenuItem,
                    copyAllMenuItem,
                    expandMenuItem,
                    clearAllMenuItem
                }
            };
        }

        private void ClearAllItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
            {
                return;
            }

            if (!(menuItem.CommandParameter is Expander expander))
            {
                return;
            }
            DumpDataStack.Children.Clear();
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