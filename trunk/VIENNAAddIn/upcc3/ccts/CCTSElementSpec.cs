// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class CCTSElementSpec : CCTSSpec
    {
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

    public class TaggedValueAttribute : Attribute
    {
        public TaggedValueAttribute() : this(TaggedValues.Undefined)
        {
        }

        public TaggedValueAttribute(TaggedValues key)
        {
            Key = key;
        }

        public TaggedValues Key { get; private set; }
    }
}