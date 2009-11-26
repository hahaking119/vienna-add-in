using System;
using System.Collections.Generic;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateABIE
    {
        internal IAbie OriginalABIE { get; set; }
        internal Boolean Checked { get; set; }
        private Dictionary<string, PotentialASBIE> PotentialASBIEs;

        public CandidateABIE(IAbie originalABIE)
        {
            OriginalABIE = originalABIE;
            Checked = false;
        }
    }
}