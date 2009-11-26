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
        public string Name { get; set; }

        public IBdt Bdt { get; set; }

        public string LowerBound { get; set; }
        public string UpperBound { get; set; }

        #region Tagged Values

        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }
        public string SequencingKey { get; set; }
        public IEnumerable<string> UsageRules { get; set; }

        #endregion

        public static BbieSpec CloneBCC(IBcc bcc, IBdt bdt)
        {
            return new BbieSpec
                   {
                       Name = bcc.Name,
                       Bdt = bdt,
                       LowerBound = bcc.LowerBound,
                       UpperBound = bcc.UpperBound,
                       BusinessTerms = new List<string>(bcc.BusinessTerms),
                       Definition = bcc.Definition,
                       DictionaryEntryName = bcc.DictionaryEntryName,
                       LanguageCode = bcc.LanguageCode,
                       SequencingKey = bcc.SequencingKey,
                       UniqueIdentifier = bcc.UniqueIdentifier,
                       UsageRules = new List<string>(bcc.UsageRules),
                       VersionIdentifier = bcc.VersionIdentifier,
                   };
        }

        public static BbieSpec CloneBbie(IBbie bbie)
        {
            return new BbieSpec
                   {
                       Name = bbie.Name,
                       DictionaryEntryName = bbie.DictionaryEntryName,
                       Definition = bbie.Definition,
                       UniqueIdentifier = bbie.UniqueIdentifier,
                       VersionIdentifier = bbie.VersionIdentifier,
                       LanguageCode = bbie.LanguageCode,
                       BusinessTerms = new List<string>(bbie.BusinessTerms),
                       UsageRules = new List<string>(bbie.UsageRules),
                       SequencingKey = bbie.SequencingKey,
                       Bdt = bbie.Type,
                       LowerBound = bbie.LowerBound,
                       UpperBound = bbie.UpperBound,
                   };
        }
    }
}