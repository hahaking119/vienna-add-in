using System;
using System.Collections.Generic;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateAbie
    {
        internal IAbie OriginalABIE { get; set; }
        internal Boolean Checked { get; set; }
        private Dictionary<string, PotentialAsbie> PotentialASBIEs;

        public CandidateAbie(IAbie originalABIE)
        {
            OriginalABIE = originalABIE;
            Checked = false;
        }
    }
}