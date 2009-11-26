using System.Collections.Generic;

namespace CctsRepository.PrimLibrary
{
    public class PRIMSpec
    {
        public PRIMSpec(IPRIM prim)
        {
            Name = prim.Name;
            DictionaryEntryName = prim.DictionaryEntryName;
            Definition = prim.Definition;
            UniqueIdentifier = prim.UniqueIdentifier;
            VersionIdentifier = prim.VersionIdentifier;
            LanguageCode = prim.LanguageCode;
            BusinessTerms = new List<string>(prim.BusinessTerms);

            Pattern = prim.Pattern;
            FractionDigits = prim.FractionDigits;
            Length = prim.Length;
            MaxExclusive = prim.MaxExclusive;
            MaxInclusive = prim.MaxInclusive;
            MaxLength = prim.MaxLength;
            MinExclusive = prim.MinExclusive;
            MinInclusive = prim.MinInclusive;
            MinLength = prim.MinLength;
            TotalDigits = prim.TotalDigits;
            WhiteSpace = prim.WhiteSpace;
            IsEquivalentTo = prim.IsEquivalentTo;
        }

        public PRIMSpec()
        {
        }

        public string Pattern { get; set; }
        public string FractionDigits { get; set; }
        public string Length { get; set; }
        public string MaxExclusive { get; set; }
        public string MaxInclusive { get; set; }
        public string MaxLength { get; set; }
        public string MinExclusive { get; set; }
        public string MinInclusive { get; set; }
        public string MinLength { get; set; }
        public string TotalDigits { get; set; }
        public string WhiteSpace { get; set; }

        public IPRIM IsEquivalentTo { get; set; }
        public string Name { get; set; }
        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }
    }
}