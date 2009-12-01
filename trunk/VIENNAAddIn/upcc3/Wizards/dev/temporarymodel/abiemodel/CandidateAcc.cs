using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateAcc
    {
        private IAcc OriginalAcc { get; set;}

        private List<CandidateAbie> candidateAbies;
        private List<CandidateBcc> candidateBccs;

        internal CandidateAcc(IAcc initAcc)
        {
            OriginalAcc = initAcc;

            candidateBccs = new List<CandidateBcc>();

            candidateAbies = new List<CandidateAbie>();            
        }
    }
}