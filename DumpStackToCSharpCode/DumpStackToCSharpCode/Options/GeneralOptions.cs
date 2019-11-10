using System;
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
        [DisplayName("Automatically expand")]
        [Description("Automatically expand objects with actual c# code dump")]
        [DefaultValue(false)]
        public bool AutomaticallyExpand { get; set; } = false;

        [Category("General")]
        [DisplayName("Generate type with namespace")]
        [Description("Generate type with namespace")]
        [DefaultValue(false)]
        public bool GenerateTypeWithNamespace { get; set; } = false;

        [Category("General")]
        [DisplayName("Max object depth")]
        [Description("Max object depth to dump")]
        [DefaultValue(10)]
        public int MaxObjectDepth { get; set; } = 10;

        [Category("General")]
        [DisplayName("Max objects to analyze")]
        [Description("Max objects to analyze on stack (equivalent to iteration count)")]
        [DefaultValue(400)]
        public int MaxObjectsToAnalyze { get; set; } = 400;

        [Category("General")]
        [DisplayName("Max generation time")]
        [Description("After this timespan generation will be stopped")]
        [DefaultValue(400)]
        public TimeSpan MaxGenerationTime { get; set; } = TimeSpan.FromSeconds(10);

        [Category("General")]
        [DisplayName("Clear dump on start")]
        [Description("Dump from previous session will be cleared on start")]
        [DefaultValue(false)]
        public bool ClearControlsOnStart { get; set; } = false;
    }
}
