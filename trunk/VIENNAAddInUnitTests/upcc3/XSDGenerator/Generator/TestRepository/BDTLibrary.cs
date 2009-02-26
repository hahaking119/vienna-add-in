using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BDTLibrary : Library
    {
        public BDTLibrary(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "BDTLibrary"; }
        }

        public List<BDT> BDTs
        {
            set { elements.AddRange(value.ConvertAll(e => (EAElement) e)); }
        }
    }
}