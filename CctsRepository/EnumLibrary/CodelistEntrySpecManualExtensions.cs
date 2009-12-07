namespace CctsRepository.EnumLibrary
{
    public partial class CodelistEntrySpec
    {
        public CodelistEntrySpec(ICodelistEntry codelistEntry)
        {
            Name = codelistEntry.Name;
            CodeName = codelistEntry.CodeName;
            Status = codelistEntry.Status;
        }

        public CodelistEntrySpec()
        {
        }
    }
}