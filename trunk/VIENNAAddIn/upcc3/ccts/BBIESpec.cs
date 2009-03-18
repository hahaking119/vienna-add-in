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
    public class BBIESpec : BIESpec
    {
        [TaggedValue(TaggedValues.SequencingKey)]
        public string SequencingKey { get; set; }

        public IBDT Type { get; set; }

        public static BBIESpec CloneBCC(IBCC bcc, IBDT type)
        {
            return new BBIESpec
                   {
                       BusinessTerms = new List<string>(bcc.BusinessTerms),
                       Definition = bcc.Definition,
                       DictionaryEntryName = bcc.DictionaryEntryName,
                       LanguageCode = bcc.LanguageCode,
                       Name = bcc.Name,
                       SequencingKey = bcc.SequencingKey,
                       Type = type,
                       UniqueIdentifier = bcc.UniqueIdentifier,
                       UsageRules = bcc.UsageRules,
                       VersionIdentifier = bcc.VersionIdentifier,
                   };
        }
    }
}