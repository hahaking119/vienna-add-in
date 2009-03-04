using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BIELibrary : UpccClassLibrary
    {
        public List<ABIE> ABIEs
        {
            set { AddClasses(value); }
        }

        public override string GetStereotype()
        {
            return "BIELibrary";
        }
    }
}