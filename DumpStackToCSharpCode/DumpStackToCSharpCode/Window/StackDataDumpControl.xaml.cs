using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Windows.Input;

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
        public const int DefaultMaxObjectDepth = 5;
        /// <summary>
        /// Initializes a new instance of the <see cref="StackDataDumpControl"/> class.
        /// </summary>
        /// <param name="stackDataDumpText"></param>
        public StackDataDumpControl(string stackDataDumpText)
        {
            this.InitializeComponent();
            this.StackDumpText.Text = stackDataDumpText;
            if (string.IsNullOrEmpty(MaxDepth.Text))
            {
                MaxDepth.Text = DefaultMaxObjectDepth.ToString();
            }
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
            Clipboard.SetText(StackDumpText.Text);
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^\d{1,2}$");
            var maxObjectDepth = MaxDepth.Text + e.Text;
            e.Handled = string.IsNullOrEmpty(maxObjectDepth) || !regex.IsMatch(maxObjectDepth);
        }

        private void MaxDepth_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (textBox.Text.Length < 1)
            {
                textBox.Text = DefaultMaxObjectDepth.ToString();
            }
        }
    }
}