using System.Collections.Generic;

namespace CctsRepository.cdt
{
    public interface ICDTSupplementaryComponent : ICCTSElement
    {
        ICDT CDT { get; }
        IBasicType BasicType { get; }
        bool ModificationAllowedIndicator { get; }
        string UpperBound { get; }
        string LowerBound { get; }
        IEnumerable<string> UsageRules { get; }
        bool IsOptional();
    }
}