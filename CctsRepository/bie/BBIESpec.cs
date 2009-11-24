// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.bdt;
using CctsRepository.cc;

namespace CctsRepository.bie
{
    public class BBIESpec : CCTSElementSpec
    {
        public BBIESpec(IBBIE bbie) : base(bbie)
        {
            UsageRules = new List<string>(bbie.UsageRules);
            SequencingKey = bbie.SequencingKey;
            Type = bbie.Type;
            LowerBound = bbie.LowerBound;
            UpperBound = bbie.UpperBound;
        }

        public BBIESpec()
        {
        }

        public string SequencingKey { get; set; }

        public IBDT Type { get; set; }

        public string LowerBound { get; set; }
        public string UpperBound { get; set; }
        public IEnumerable<string> UsageRules { get; set; }

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
                       UsageRules = new List<string>(bcc.UsageRules),
                       VersionIdentifier = bcc.VersionIdentifier,
                       LowerBound = bcc.LowerBound,
                       UpperBound = bcc.UpperBound,
                   };
        }
    }
}