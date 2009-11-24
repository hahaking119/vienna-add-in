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
    public class ABIESpec
    {
        private List<ASBIESpec> asbies;
        private List<BBIESpec> bbies;

        public ABIESpec(IABIE abie)
        {
            Name = abie.Name;
            DictionaryEntryName = abie.DictionaryEntryName;
            Definition = abie.Definition;
            UniqueIdentifier = abie.UniqueIdentifier;
            VersionIdentifier = abie.VersionIdentifier;
            LanguageCode = abie.LanguageCode;
            BusinessTerms = new List<string>(abie.BusinessTerms);
            UsageRules = new List<string>(abie.UsageRules);
            BBIEs = abie.BBIEs.Convert(bbie => new BBIESpec(bbie));
            ASBIEs = abie.ASBIEs.Convert(asbie => new ASBIESpec(asbie));
            IsEquivalentTo = abie.IsEquivalentTo;
            BasedOn = abie.BasedOn;
        }

        public ABIESpec()
        {
            bbies = new List<BBIESpec>();
            asbies = new List<ASBIESpec>();
        }

        public IEnumerable<BBIESpec> BBIEs
        {
            get { return bbies; }
            set { bbies = new List<BBIESpec>(value); }
        }

        public IEnumerable<ASBIESpec> ASBIEs
        {
            get { return asbies; }
            set { asbies = new List<ASBIESpec>(value); }
        }

        public IABIE IsEquivalentTo { get; set; }

        public IACC BasedOn { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
        public string Name { get; set; }
        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }

        public void AddASBIE(ASBIESpec spec)
        {
            asbies.Add(spec);
        }

        public void RemoveASBIE(string name)
        {
            asbies.RemoveAll(asbie => asbie.Name == name);
        }

        public void AddBBIE(BBIESpec spec)
        {
            bbies.Add(spec);
        }

        public void RemoveBBIE(string name)
        {
            bbies.RemoveAll(bbie => bbie.Name == name);
        }
    }
}