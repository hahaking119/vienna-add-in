namespace CctsRepository.EnumLibrary
{
    public interface ICodelistEntry
    {
        string Name { get; }
        string CodeName { get; }
        string Status { get; }
    }
}