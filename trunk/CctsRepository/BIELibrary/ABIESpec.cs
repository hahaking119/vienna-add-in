// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.CcLibrary;
using VIENNAAddInUtils;

namespace CctsRepository.BieLibrary
{
    public class AbieSpec
    {
        private List<AsbieSpec> asbies;
        private List<BbieSpec> bbies;

        public AbieSpec(IAbie abie)
        {
            Name = abie.Name;
            DictionaryEntryName = abie.DictionaryEntryName;
            Definition = abie.Definition;
            UniqueIdentifier = abie.UniqueIdentifier;
            VersionIdentifier = abie.VersionIdentifier;
            LanguageCode = abie.LanguageCode;
            BusinessTerms = new List<string>(abie.BusinessTerms);
            UsageRules = new List<string>(abie.UsageRules);
            BBIEs = abie.BBIEs.Convert(bbie => new BbieSpec(bbie));
            ASBIEs = abie.ASBIEs.Convert(asbie => new AsbieSpec(asbie));
            IsEquivalentTo = abie.IsEquivalentTo;
            BasedOn = abie.BasedOn;
        }

        public AbieSpec()
        {
            bbies = new List<BbieSpec>();
            asbies = new List<AsbieSpec>();
        }

        public IEnumerable<BbieSpec> BBIEs
        {
            get { return bbies; }
            set { bbies = new List<BbieSpec>(value); }
        }

        public IEnumerable<AsbieSpec> ASBIEs
        {
            get { return asbies; }
            set { asbies = new List<AsbieSpec>(value); }
        }

        public IAbie IsEquivalentTo { get; set; }

        public IACC BasedOn { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
        public string Name { get; set; }
        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }

        public void AddASBIE(AsbieSpec spec)
        {
            asbies.Add(spec);
        }

        public void RemoveASBIE(string name)
        {
            asbies.RemoveAll(asbie => asbie.Name == name);
        }

        public void AddBBIE(BbieSpec spec)
        {
            bbies.Add(spec);
        }

        public void RemoveBBIE(string name)
        {
            bbies.RemoveAll(bbie => bbie.Name == name);
        }
    }
}