using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class PRIMLibrary : UpccClassLibrary
    {
        public override string GetStereotype()
        {
            return "PRIMLibrary";
        }

        public List<PRIM> PRIMs
        {
            set { AddClasses(value); }
        }
    }
}