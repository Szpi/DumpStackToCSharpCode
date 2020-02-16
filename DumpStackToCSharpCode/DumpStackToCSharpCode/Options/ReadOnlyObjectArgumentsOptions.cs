using RuntimeTestDataCollector.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DumpStackToCSharpCode.Options
{
    [Serializable]
    class ReadOnlyObjectArgumentsOptions : BaseOptionModel<ReadOnlyObjectArgumentsOptions>
    {
        [Category("Readonly class arguments")]
        [DisplayName("Object Descriptions")]
        [Description("Please specify: Class name that should be initialized by constructor and constructor arguments separated by ',' or ';' that matches objects members")]
        public List<ReadOnlyObjectDescription> ObjectDescription { get; set; } = new List<ReadOnlyObjectDescription>();     
    }

    [Serializable]
    public class ReadOnlyObjectDescription
    {
        [Category("Readonly class arguments")]
        [DisplayName("Class Name")]
        [Description("Class name that should be initialized by constructor")]
        [DefaultValue("")]
        public string ClassName { get; set; }

        [Category("Readonly class arguments")]
        [DisplayName("Constructor arguments")]
        [Description("Specify constructor arguments separated by , or ; that matches objects members")]
        [DefaultValue("")]
        public string Arguments { get; set; }
    }
}
