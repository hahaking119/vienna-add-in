using System.Collections.Generic;

namespace CctsRepository.PrimLibrary
{
    public partial class PrimSpec
    {
        public PrimSpec(IPrim prim)
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
            MaximumExclusive = prim.MaximumExclusive;
            MaximumInclusive = prim.MaximumInclusive;
            MaximumLength = prim.MaximumLength;
            MinimumExclusive = prim.MinimumExclusive;
            MinimumInclusive = prim.MinimumInclusive;
            MinimumLength = prim.MinimumLength;
            TotalDigits = prim.TotalDigits;
            WhiteSpace = prim.WhiteSpace;
            IsEquivalentTo = prim.IsEquivalentTo;
        }

        public PrimSpec()
        {
        }
    }
}