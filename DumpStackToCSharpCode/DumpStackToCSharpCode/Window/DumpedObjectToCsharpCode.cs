namespace DumpStackToCSharpCode.Window
{
    public class DumpedObjectToCsharpCode
    {
        public string Name { get; }
        public string CsharpCode { get; }
        public string ErrorMessage { get; }

        public DumpedObjectToCsharpCode(string name, string csharpCode, string errorMessage)
        {
            Name = name;
            CsharpCode = csharpCode;
            ErrorMessage = errorMessage;
        }
    }
}