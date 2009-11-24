using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialASBIE
    {
        internal Boolean Checked { get; set; }
        internal IASCC BasedOn { get; set; }

        public PotentialASBIE(IASCC basedOn)
        {
            BasedOn = basedOn;
            Checked = false;
        }
    }
}