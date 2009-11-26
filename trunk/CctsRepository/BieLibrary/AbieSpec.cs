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
        private List<AsbieSpec> asbies = new List<AsbieSpec>();
        private List<BbieSpec> bbies = new List<BbieSpec>();

        public IEnumerable<BbieSpec> Bbies
        {
            get { return bbies; }
            set { bbies = new List<BbieSpec>(value); }
        }

        public IEnumerable<AsbieSpec> Asbies
        {
            get { return asbies; }
            set { asbies = new List<AsbieSpec>(value); }
        }

        public IAbie IsEquivalentTo { get; set; }

        public IAcc BasedOn { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
        public string Name { get; set; }
        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }

        public static AbieSpec CloneAbie(IAbie abie)
        {
            return new AbieSpec
                   {
                       Name = abie.Name,
                       DictionaryEntryName = abie.DictionaryEntryName,
                       Definition = abie.Definition,
                       UniqueIdentifier = abie.UniqueIdentifier,
                       VersionIdentifier = abie.VersionIdentifier,
                       LanguageCode = abie.LanguageCode,
                       BusinessTerms = new List<string>(abie.BusinessTerms),
                       UsageRules = new List<string>(abie.UsageRules),
                       Bbies = abie.BBIEs.Convert(bbie => BbieSpec.CloneBbie(bbie)),
                       Asbies = abie.ASBIEs.Convert(asbie => new AsbieSpec(asbie)),
                       IsEquivalentTo = abie.IsEquivalentTo,
                       BasedOn = abie.BasedOn,
                   }
                ;
        }

        public void AddAsbie(AsbieSpec spec)
        {
            asbies.Add(spec);
        }

        public void RemoveAsbie(string name)
        {
            asbies.RemoveAll(asbie => asbie.Name == name);
        }

        public void AddBbie(BbieSpec spec)
        {
            bbies.Add(spec);
        }

        public void RemoveBbie(string name)
        {
            bbies.RemoveAll(bbie => bbie.Name == name);
        }
    }
}