namespace DumpStackToCSharpCode.ObjectInitializationGeneration
{
    public class VariableDeclarationManager
    {
        private const string VarDeclaration = "var";
        private readonly bool _shouldUseConcreteType;
        public VariableDeclarationManager(bool shouldUseConcreteType)
        {
            _shouldUseConcreteType = shouldUseConcreteType;
        }

        public string GetDeclarationType(string type)
        {
            return _shouldUseConcreteType ? type : VarDeclaration;
        }
    }
}
