using System.ComponentModel;

namespace RuntimeTestDataCollector.Options
{
    internal class GeneralOptions : BaseOptionModel<GeneralOptions>
    {
        [Category("General")]
        [DisplayName("Automatically refresh")]
        [Description("Automatically refresh after debugger context change")]
        [DefaultValue(false)]
        public bool AutomaticallyRefresh { get; set; } = false;

        [Category("General")]
        [DisplayName("Max object depth")]
        [Description("Max object depth to dump")]
        [DefaultValue(10)]
        public int MaxObjectDepth { get; set; } = 10;
    }
}
