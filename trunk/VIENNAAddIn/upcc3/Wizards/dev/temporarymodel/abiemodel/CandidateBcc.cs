using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBcc
    {
        private string Name { get; set; }
        private bool Checked { get; set; }        
        private IBcc OriginalBcc { get; set; }
        private List<PotentialBbie> potentialBbies;        

        public CandidateBcc(IBcc initOriginalBcc)
        {
            Name = initOriginalBcc.Name;
            Checked = false;
            OriginalBcc = initOriginalBcc;            
            potentialBbies = new List<PotentialBbie>();
        }
    }
}