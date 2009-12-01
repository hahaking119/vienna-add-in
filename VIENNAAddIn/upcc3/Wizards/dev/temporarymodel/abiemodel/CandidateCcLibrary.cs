using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateCcLibrary
    {
        private ICcLibrary OriginalCcLibrary { get; set; }
        private List<CandidateAcc> CandidateAccs { get; set; }

        public CandidateCcLibrary(ICcLibrary initCcLibrary)
        {
            OriginalCcLibrary = initCcLibrary;

            CandidateAccs = new List<CandidateAcc>();
        }
    }
}