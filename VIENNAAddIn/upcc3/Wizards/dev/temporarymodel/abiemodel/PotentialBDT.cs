using System;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBDT
    {
        internal string Name { get; set; }
        internal Boolean Checked { get; set; }
        internal IBdt OriginalBDT { get; set; }

        public PotentialBDT(string name)
        {
            Name = name;
            Checked = false;
        }

        public PotentialBDT(string name, IBdt originalBDT)
            : this(name)
        {
            OriginalBDT = originalBDT;
        }
    }
}