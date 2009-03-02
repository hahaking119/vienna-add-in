using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class DT : UpccClass
    {
        public CON CON
        {
            set { AddAttribute(value); }
        }

        public List<SUP> SUPs
        {
            set { AddAttributes(value); }
        }
    }
}