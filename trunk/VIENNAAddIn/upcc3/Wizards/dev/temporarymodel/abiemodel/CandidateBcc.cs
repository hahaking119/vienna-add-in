using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBcc
    {
        internal string Name { get; set; }
        internal IBcc OriginalBCC { get; set; }
        internal Boolean Checked { get; set; }
        internal Dictionary<string, PotentialBbie> PotentialBBIEs { get; set;}

        public CandidateBcc(IBcc originalBCC)
        {
            Name = originalBCC.Name;
            OriginalBCC = originalBCC;
            Checked = false;
            PotentialBBIEs = new Dictionary<string, PotentialBbie>();
        }
    }
}