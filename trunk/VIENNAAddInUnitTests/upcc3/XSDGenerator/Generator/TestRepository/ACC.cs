using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class ACC : UpccClass
    {
        public ACC(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "ACC"; }
        }

        public List<BCC> BCCs
        {
            set { AddAttributes(value); }
        }
    }
}