namespace RuntimeTestDataCollector.Window
{
    public class DumpedObjectToCsharpCode
    {
        public string Name { get; }
        public string CsharpCode { get; }

        public DumpedObjectToCsharpCode(string name, string csharpCode)
        {
            Name = name;
            CsharpCode = csharpCode;
        }
    }
}