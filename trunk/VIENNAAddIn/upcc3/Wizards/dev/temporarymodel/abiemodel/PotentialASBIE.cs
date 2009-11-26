using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialASBIE
    {
        internal Boolean Checked { get; set; }
        internal IAscc BasedOn { get; set; }

        public PotentialASBIE(IAscc basedOn)
        {
            BasedOn = basedOn;
            Checked = false;
        }
    }
}