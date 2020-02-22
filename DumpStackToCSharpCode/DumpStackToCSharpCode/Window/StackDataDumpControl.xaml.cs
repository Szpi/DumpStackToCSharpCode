using RuntimeTestDataCollector.Command;
using RuntimeTestDataCollector.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace RuntimeTestDataCollector.Window
{
    using DumpStackToCSharpCode.CurrentStack;
    using DumpStackToCSharpCode.Options;
    using DumpStackToCSharpCode.Window;
    using EnvDTE;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using static RuntimeTestDataCollector.Options.DialogPageProvider;

    /// <summary>
    /// Interaction logic for StackDataDumpControl.
    /// </summary>
    public partial class StackDataDumpControl : UserControl
    {
        private const string ExpandAll = "Expand all";
        private const string CollapseAll = "Collapse all";

        private const int GeneralTabIndex = 0;
        public const int LocalsTabIndex = 1;

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
            GeneralOptions.Instance.OnSettingsSave += OnOptionsSave;
            ReadOnlyObjectArgumentsOptions.Instance.OnSettingsSave += OnReadOnlyObjectArgumentsOptionsSave;

            DumpStackToCSharpCodeCommand.Instance.SubscribeForReadOnlyObjectArgumentsPageProviderEvents(
                OnReadOnlyObjectArgumentsPageProviderActivated,
                OnReadOnlyObjectArgumentsOptionsSave);

            GenerateArguments();
        }

        private void OnReadOnlyObjectArgumentsPageProviderActivated(object sender, bool e)
        {
            SaveReadonlyObjectArguments();
        }

        private void OnReadOnlyObjectArgumentsOptionsSave(object sender, bool e)
        {
            ReadOnlyObjectArgumentsOptions.Instance.Load();
            for (int i = 0; i < Class.Children?.Count; i++)
            {
                var control = Class.Children[i];
                if (control is TextBox classTextBox)
                {
                    classTextBox.Text = string.Empty;
                }

                if (Arguments.Children[i] is TextBox argumentsTextBox)
                {
                    argumentsTextBox.Text = string.Empty;
                }
            }
            GenerateArguments();
        }

        private async void OnOptionsSave(object sender, bool value)
        {
            DumpStackToCSharpCodeCommand.Instance.UnSubscribeForDebuggerContextChange(OnDebuggerContextChange);
            if (GeneralOptions.Instance.AutomaticallyRefreshLocals)
            {
                DumpStackToCSharpCodeCommand.Instance.SubscribeForDebuggerContextChange(OnDebuggerContextChange);
            }

            await DumpStackToCSharpCodeCommand.Instance.OnSettingsSaveAsync();
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
            var checkedLocals = new List<string>();
            foreach (CheckBox checkBox in LocalsStack.Children)
            {
                if (!checkBox.IsChecked.GetValueOrDefault())
                {
                    continue;
                }
                checkedLocals.Add(checkBox.Name);
            }
            var arg = new ChosenLocalsEventArgs(checkedLocals);

            DumpStackToCSharpCodeCommand.Instance.Execute(this, arg);
            MainTabControl.SelectedIndex = GeneralTabIndex;
        }
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            Arguments.Children.Add(new TextBox()
            {
                Background = ClassPrototype.Background,
                Foreground = ClassPrototype.Foreground,
                FontFamily = ClassPrototype.FontFamily,
                Width = ClassPrototype.Width,
                Margin = ClassPrototype.Margin,
                Height = ClassPrototype.Height
            });
            Class.Children.Add(new TextBox()
            {
                Background = ArgumentsPrototype.Background,
                Foreground = ArgumentsPrototype.Foreground,
                FontFamily = ArgumentsPrototype.FontFamily,
                Width = ArgumentsPrototype.Width,
                Margin = ArgumentsPrototype.Margin,
                Height = ArgumentsPrototype.Height
            });
        }
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void SaveArguments_Click(object sender, RoutedEventArgs e)
        {
            SaveReadonlyObjectArguments();
        }

        private void GenerateArguments()
        {
            for (int i = 0; i < ReadOnlyObjectArgumentsOptions.Instance?.ObjectDescription?.Count; i++)
            {
                var objectDescription = ReadOnlyObjectArgumentsOptions.Instance.ObjectDescription[i];
                if (string.IsNullOrWhiteSpace(objectDescription.ClassName) || string.IsNullOrWhiteSpace(objectDescription.Arguments))
                {
                    continue;
                }

                if (i < Arguments.Children.Count)
                {
                    var control = Class.Children[i];
                    if (control is TextBox classTextBox && string.IsNullOrWhiteSpace(classTextBox.Text))
                    {
                        classTextBox.Text = objectDescription.ClassName;
                        if (Arguments.Children[i] is TextBox argumentsTextBox)
                        {
                            argumentsTextBox.Text = objectDescription.Arguments;
                        }
                    }

                    continue;
                }

                Arguments.Children.Add(new TextBox()
                {
                    Background = ClassPrototype.Background,
                    Foreground = ClassPrototype.Foreground,
                    FontFamily = ClassPrototype.FontFamily,
                    Width = ClassPrototype.Width,
                    Margin = ClassPrototype.Margin,
                    Text = objectDescription.ClassName
                });
                Class.Children.Add(new TextBox()
                {
                    Background = ArgumentsPrototype.Background,
                    Foreground = ArgumentsPrototype.Foreground,
                    FontFamily = ArgumentsPrototype.FontFamily,
                    Width = ArgumentsPrototype.Width,
                    Margin = ArgumentsPrototype.Margin,
                    Text = objectDescription.Arguments
                });

            }
            if (Class.Children.Count - ReadOnlyObjectArgumentsOptions.Instance?.ObjectDescription?.Count == 0)
            {
                AddRow_Click(this, null);
            }
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
                if (GeneralOptions.Instance.AutomaticallyRefreshLocals)
                {
                    DumpStackToCSharpCodeCommand.Instance.UnSubscribeForDebuggerContextChange(OnDebuggerContextChange);
                }

                DumpStackToCSharpCodeCommand.Instance.ResetCurrentStack();
                return;
            }

            SaveReadonlyObjectArguments();

            if (GeneralOptions.Instance.AutomaticallyRefreshLocals)
            {
                DumpStackToCSharpCodeCommand.Instance.SubscribeForDebuggerContextChange(OnDebuggerContextChange);
            }
            CreateLocls();
        }

        private void SaveReadonlyObjectArguments()
        {
            if (!GeneralOptions.Instance.AutomaticallySaveConsturctorParameters)
            {
                return;
            }

            ReadOnlyObjectArgumentsOptions.Instance.ObjectDescription = new List<ReadOnlyObjectDescription>();
            for (int i = 0; i < Class.Children.Count; i++)
            {
                if (Class.Children[i] is TextBox classTextBox && !string.IsNullOrWhiteSpace(classTextBox.Text)
                    && Arguments.Children[i] is TextBox argumentsTextBox && !string.IsNullOrWhiteSpace(argumentsTextBox.Text))
                {
                    ReadOnlyObjectArgumentsOptions.Instance.ObjectDescription.Add(new ReadOnlyObjectDescription
                    {
                        ClassName = classTextBox.Text,
                        Arguments = argumentsTextBox.Text,
                    });
                }
            }
            ReadOnlyObjectArgumentsOptions.Instance.Save();
        }

        private void CreateLocls()
        {
            LocalsStack.Children.Clear();

            var locals = DumpStackToCSharpCodeCommand.Instance.GetCurrentStack();
            CreateLocals(locals);
        }

        private void OnDebuggerContextChange(Process newprocess, Program newprogram, Thread newthread, StackFrame newstackframe)
        {
            try
            {
                CreateLocls();
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        private void CreateLocals(IReadOnlyCollection<CurrentExpressionOnStack> locals)
        {
            foreach (var local in locals)
            {
                var localVariable = new CheckBox()
                {
                    Content = local.Name,
                    Name = local.Name,
                    IsChecked = false,
                    Background = AutomaticallyRefresh.Background,
                    FontFamily = AutomaticallyRefresh.FontFamily,
                    Foreground = AutomaticallyRefresh.Foreground,
                    FocusVisualStyle = AutomaticallyRefresh.FocusVisualStyle,
                    BorderBrush = AutomaticallyRefresh.BorderBrush
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
            CreateExpanderContextMenuForContent(expander);
            return expander;
        }
        private void CreateExpanderContextMenuForContent(Expander expander)
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
                Header = CollapseAll,
                CommandParameter = expander
            };
            expandMenuItem.Click += ExpandMenuItemDontChangeHeader_OnClick;

            var clearAllMenuItem = new MenuItem()
            {
                Header = "Clear all",
                CommandParameter = expander
            };
            clearAllMenuItem.Click += ClearAllItem_OnClick;

            (expander.Content as TextBox).ContextMenu = new ContextMenu()
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

        private void ExpandMenuItemDontChangeHeader_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
            {
                return;
            }

            if (!(menuItem.CommandParameter is Expander expander))
            {
                return;
            }

            foreach (MenuItem item in expander.ContextMenu.Items)
            {
                var content = item.Header as string;
                if (content == CollapseAll || content == ExpandAll)
                {
                    item.Header = expander.IsExpanded ? CollapseAll : ExpandAll;
                }
            }
            
            ExecuteExpandCollapseContextItemDontChangeHeader(menuItem);
        }

        private void ExecuteExpandCollapseContextItemDontChangeHeader(MenuItem menuItem)
        {
            foreach (Expander child in DumpDataStack.Children)
            {
                child.IsExpanded = !child.IsExpanded;
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public void LogException(Exception exception)
        {
            if (exception is AggregateException aggregateException)
            {
                ErrorMessage.Text = "Exception: " + aggregateException.Flatten().Message;
            }
            else
            {
                ErrorMessage.Text = "Exception: " + exception.Message;
            }

            ErrorMessage.Text += Environment.NewLine + exception.StackTrace;
            ErrorMessageRow.Height = new GridLength(1, GridUnitType.Auto);
        }
    }
}