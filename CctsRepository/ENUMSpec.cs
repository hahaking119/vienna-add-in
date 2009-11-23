using System.Collections.Generic;
using VIENNAAddInUtils;

namespace CctsRepository
{
    public class ENUMSpec : CCTSElementSpec
    {
        private List<CodelistEntrySpec> codelistEntries;

        public ENUMSpec(IENUM @enum) : base(@enum)
        {
            CodeListAgencyIdentifier = @enum.CodeListAgencyIdentifier;
            CodeListAgencyName = @enum.CodeListAgencyName;
            CodeListIdentifier = @enum.CodeListIdentifier;
            CodeListName = @enum.CodeListName;
            ModificationAllowedIndicator = @enum.ModificationAllowedIndicator;
            EnumerationURI = @enum.EnumerationURI;
            IsEquivalentTo = @enum.IsEquivalentTo;
            codelistEntries = new List<CodelistEntrySpec>(@enum.CodelistEntries.Convert(codelistEntry => new CodelistEntrySpec(codelistEntry)));
        }

        public ENUMSpec()
        {
            codelistEntries = new List<CodelistEntrySpec>();
        }

        public string CodeListAgencyIdentifier { get; set; }
        public string CodeListAgencyName { get; set; }
        public string CodeListIdentifier { get; set; }
        public string CodeListName { get; set; }
        public string EnumerationURI { get; set; }
        public bool ModificationAllowedIndicator { get; set; }
        public string RestrictedPrimitive { get; set; }
        public string Status { get; set; }

        public IENUM IsEquivalentTo { get; set; }

        public IEnumerable<CodelistEntrySpec> CodelistEntries
        {
            get { return codelistEntries; }
            set { codelistEntries = new List<CodelistEntrySpec>(value); }
        }

        public void AddCodelistEntry(CodelistEntrySpec spec)
        {
            codelistEntries.Add(spec);
        }

        public void RemoveBBIE(string name)
        {
            codelistEntries.RemoveAll(entry => entry.Name == name);
        }
    }
}