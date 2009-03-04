using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class CCLibrary : UpccClassLibrary
    {
        public List<ACC> ACCs
        {
            set { AddClasses(value); }
        }

        public override string GetStereotype()
        {
            return "CCLibrary";
        }
    }
}