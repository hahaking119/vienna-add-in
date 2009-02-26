using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class CDTLibrary : Library
    {
        public CDTLibrary(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "CDTLibrary"; }
        }

        public List<CDT> CDTs
        {
            set { elements.AddRange(value.ConvertAll(e => (EAElement) e)); }
        }
    }
}