using System;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBdt
    {
        internal string Name { get; set; }
        internal Boolean Checked { get; set; }
        internal IBdt OriginalBDT { get; set; }

        public PotentialBdt(string name)
        {
            Name = name;
            Checked = false;
        }

        public PotentialBdt(string name, IBdt originalBDT)
            : this(name)
        {
            OriginalBDT = originalBDT;
        }
    }
}