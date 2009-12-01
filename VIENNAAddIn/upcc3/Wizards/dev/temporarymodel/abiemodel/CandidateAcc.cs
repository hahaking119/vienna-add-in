using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateAcc
    {
        internal IAcc OriginalAcc { get; set;}
        internal bool Selected { get; set; }
        internal List<CandidateAbie> candidateAbies { get; set; }
        internal List<CandidateBcc> candidateBccs { get; set; }
        internal CandidateAcc(IAcc acc)
        {
            OriginalAcc = acc;
            candidateAbies = new List<CandidateAbie>();
            candidateBccs = new List<CandidateBcc>();
        }
    }
}