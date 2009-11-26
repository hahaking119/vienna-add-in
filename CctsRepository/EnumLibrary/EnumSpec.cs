using System.Collections.Generic;
using VIENNAAddInUtils;

namespace CctsRepository.EnumLibrary
{
    public class EnumSpec
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

        public string CodeListAgencyIdentifier { get; set; }
        public string CodeListAgencyName { get; set; }
        public string CodeListIdentifier { get; set; }
        public string CodeListName { get; set; }
        public string EnumerationURI { get; set; }
        public bool ModificationAllowedIndicator { get; set; }
        public string RestrictedPrimitive { get; set; }
        public string Status { get; set; }

        public IEnum IsEquivalentTo { get; set; }

        public IEnumerable<CodelistEntrySpec> CodelistEntries
        {
            get { return codelistEntries; }
            set { codelistEntries = new List<CodelistEntrySpec>(value); }
        }

        public string Name { get; set; }
        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }

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