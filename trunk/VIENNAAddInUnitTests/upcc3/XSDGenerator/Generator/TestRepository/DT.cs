using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class DT : UpccClass
    {
        protected DT(string name) : base(name)
        {
        }

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