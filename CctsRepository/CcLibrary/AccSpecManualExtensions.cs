using System.Collections.Generic;
using VIENNAAddInUtils;

namespace CctsRepository.CcLibrary
{
    public partial class AccSpec
    {
        private readonly List<AsccSpec> asccs;
        private readonly List<BccSpec> bccs;

        public AccSpec(IAcc acc)
        {
            Name = acc.Name;
            DictionaryEntryName = acc.DictionaryEntryName;
            Definition = acc.Definition;
            UniqueIdentifier = acc.UniqueIdentifier;
            VersionIdentifier = acc.VersionIdentifier;
            LanguageCode = acc.LanguageCode;
            BusinessTerms = new List<string>(acc.BusinessTerms);
            UsageRules = new List<string>(acc.UsageRules);
            bccs = new List<BccSpec>(acc.Bccs.Convert(bcc => new BccSpec(bcc)));
            asccs = new List<AsccSpec>(acc.Asccs.Convert(ascc => new AsccSpec(ascc)));
            IsEquivalentTo = acc.IsEquivalentTo;
        }

        public AccSpec()
        {
            bccs = new List<BccSpec>();
            asccs = new List<AsccSpec>();
        }

        public IEnumerable<BccSpec> BCCs
        {
            get { return bccs; }
        }

        public IEnumerable<AsccSpec> ASCCs
        {
            get { return asccs; }
        }

        public void RemoveASCC(string name)
        {
            asccs.RemoveAll(ascc => ascc.Name == name);
        }

        public void RemoveBCC(string name)
        {
            bccs.RemoveAll(bcc => bcc.Name == name);
        }

        public void AddBCC(BccSpec bcc)
        {
            bccs.Add(bcc);
        }

        public void AddASCC(AsccSpec ascc)
        {
            asccs.Add(ascc);
        }
    }
}