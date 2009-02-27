using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class CDTLibrary : UpccClassLibrary
    {
        public CDTLibrary(string name) : base(name)
        {
        }

        public override string GetStereotype()
        {
            return "CDTLibrary";
        }

        public List<CDT> CDTs
        {
            set { AddClasses(value); }
        }
    }
}