using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class CDTLibrary : UpccClassLibrary
    {
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