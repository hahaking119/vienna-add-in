using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class PRIMLibrary : Library
    {
        public PRIMLibrary(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "PRIMLibrary"; }
        }

        public List<PRIM> PRIMs
        {
            set { elements.AddRange(value.ConvertAll(e => (EAElement)e)); }
        }
    }
}