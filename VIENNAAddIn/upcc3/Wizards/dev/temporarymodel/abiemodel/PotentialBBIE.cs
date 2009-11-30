using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBbie
    {
        internal string Name { get; set; }
        internal Boolean Checked { get; set; }
        internal Dictionary<string, PotentialBdt> PotentialBDTs { get; set; }

        public PotentialBbie(string name)
        {
            Name = name;
            Checked = false;
            PotentialBDTs = new Dictionary<string, PotentialBdt>();
        }
    }
}