using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BDTLibrary : UpccClassLibrary
    {
        public List<BDT> BDTs
        {
            set { AddClasses(value); }
        }

        public override string GetStereotype()
        {
            return "BDTLibrary";
        }
    }
}