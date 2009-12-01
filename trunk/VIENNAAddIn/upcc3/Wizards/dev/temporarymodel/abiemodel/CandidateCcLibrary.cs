using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateCcLibrary
    {
        internal bool Selected { get; set; }
        internal ICcLibrary OriginalCcLibrary { get; set; }
        internal List<CandidateAcc> CandidateAccs { get; set; }

        public CandidateCcLibrary(ICcLibrary ccLibrary)
        {
            OriginalCcLibrary = ccLibrary;
            CandidateAccs = new List<CandidateAcc>();
        }
    }
}