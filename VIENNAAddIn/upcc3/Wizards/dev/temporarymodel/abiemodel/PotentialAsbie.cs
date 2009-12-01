using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialAsbie
    {
        internal bool Checked { get; set; }
        internal IAscc BasedOn { get; set; }

        public PotentialAsbie(IAscc basedOn)
        {
            BasedOn = basedOn;
            Checked = false;
        }
    }
}