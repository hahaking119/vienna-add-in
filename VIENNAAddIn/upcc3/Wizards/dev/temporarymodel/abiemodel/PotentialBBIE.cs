using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBBIE
    {
        internal string Name { get; set; }
        internal Boolean Checked { get; set; }
        private Dictionary<string, PotentialBDT> PotentialBDTs;

        public PotentialBBIE(string name)
        {
            Name = name;
            Checked = false;
        }
    }
}