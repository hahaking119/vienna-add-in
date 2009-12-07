using System.Collections.Generic;
using VIENNAAddInUtils;

namespace CctsRepository.EnumLibrary
{
    public partial class EnumSpec
    {
        private List<CodelistEntrySpec> codelistEntries;

        public EnumSpec(IEnum @enum)
        {
            Name = @enum.Name;
            DictionaryEntryName = @enum.DictionaryEntryName;
            Definition = @enum.Definition;
            UniqueIdentifier = @enum.UniqueIdentifier;
            VersionIdentifier = @enum.VersionIdentifier;
            LanguageCode = @enum.LanguageCode;
            BusinessTerms = new List<string>(@enum.BusinessTerms);

            CodeListAgencyIdentifier = @enum.CodeListAgencyIdentifier;
            CodeListAgencyName = @enum.CodeListAgencyName;
            CodeListIdentifier = @enum.CodeListIdentifier;
            CodeListName = @enum.CodeListName;
            ModificationAllowedIndicator = @enum.ModificationAllowedIndicator;
            EnumerationURI = @enum.EnumerationURI;
            IsEquivalentTo = @enum.IsEquivalentTo;
            codelistEntries = new List<CodelistEntrySpec>(@enum.CodelistEntries.Convert(codelistEntry => new CodelistEntrySpec(codelistEntry)));
        }

        public EnumSpec()
        {
            codelistEntries = new List<CodelistEntrySpec>();
        }

        public IEnumerable<CodelistEntrySpec> CodelistEntries
        {
            get { return codelistEntries; }
            set { codelistEntries = new List<CodelistEntrySpec>(value); }
        }

        public void AddCodelistEntry(CodelistEntrySpec spec)
        {
            codelistEntries.Add(spec);
        }

        public void RemoveCodelistEntry(string name)
        {
            codelistEntries.RemoveAll(entry => entry.Name == name);
        }
    }
}