using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBCC
    {
        internal IBCC OriginalBCC { get; set; }
        internal Boolean Checked { get; set; }
        private Dictionary<string, PotentialBBIE> PotentialBBIEs;

        public CandidateBCC(IBCC originalBCC)
        {
            OriginalBCC = originalBCC;
            Checked = false;
        }
    }
}