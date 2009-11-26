// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.CcLibrary;

namespace CctsRepository.BieLibrary
{
    public class BbieSpec
    {
        public BbieSpec(IBbie bbie)
        {
            Name = bbie.Name;
            DictionaryEntryName = bbie.DictionaryEntryName;
            Definition = bbie.Definition;
            UniqueIdentifier = bbie.UniqueIdentifier;
            VersionIdentifier = bbie.VersionIdentifier;
            LanguageCode = bbie.LanguageCode;
            BusinessTerms = new List<string>(bbie.BusinessTerms);
            UsageRules = new List<string>(bbie.UsageRules);
            SequencingKey = bbie.SequencingKey;
            Type = bbie.Type;
            LowerBound = bbie.LowerBound;
            UpperBound = bbie.UpperBound;
        }

        public BbieSpec()
        {
        }

        public string SequencingKey { get; set; }

        public IBdt Type { get; set; }

        public string LowerBound { get; set; }
        public string UpperBound { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
        public string Name { get; set; }
        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }

        public static BbieSpec CloneBCC(IBCC bcc, IBdt type)
        {
            return new BbieSpec
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