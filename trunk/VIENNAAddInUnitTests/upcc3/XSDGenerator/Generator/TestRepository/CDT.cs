using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class CDT : EAElement
    {
        public CDT(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "CDT"; }
        }
    }
}