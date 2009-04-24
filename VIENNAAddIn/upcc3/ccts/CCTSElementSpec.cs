// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public abstract class CCTSElementSpec : CCTSSpec
    {
        protected CCTSElementSpec(ICCTSElement cctsElement)
        {
            Name = cctsElement.Name;
            DictionaryEntryName = cctsElement.DictionaryEntryName;
            Definition = cctsElement.Definition;
            UniqueIdentifier = cctsElement.UniqueIdentifier;
            VersionIdentifier = cctsElement.VersionIdentifier;
            LanguageCode = cctsElement.LanguageCode;
            BusinessTerms = new List<string>(cctsElement.BusinessTerms);
        }

        protected CCTSElementSpec()
        {
        }

        public string Name { get; set; }

        [TaggedValue(TaggedValues.DictionaryEntryName)]
        public string DictionaryEntryName { get; set; }

        [TaggedValue(TaggedValues.Definition)]
        public string Definition { get; set; }

        [TaggedValue(TaggedValues.UniqueIdentifier)]
        public string UniqueIdentifier { get; set; }

        [TaggedValue(TaggedValues.VersionIdentifier)]
        public string VersionIdentifier { get; set; }

        [TaggedValue(TaggedValues.LanguageCode)]
        public string LanguageCode { get; set; }

        [TaggedValue(TaggedValues.BusinessTerm)]
        public IEnumerable<string> BusinessTerms { get; set; }
    }
}