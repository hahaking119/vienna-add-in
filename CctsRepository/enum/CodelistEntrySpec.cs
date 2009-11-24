namespace CctsRepository.@enum
{
    public class CodelistEntrySpec
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

        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Status { get; set; }
    }
}