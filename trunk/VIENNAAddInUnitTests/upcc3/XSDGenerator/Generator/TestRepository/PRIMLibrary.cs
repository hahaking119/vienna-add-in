using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class PRIMLibrary : UpccPackage
    {
        public PRIMLibrary(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "PRIMLibrary"; }
        }

        public List<PRIM> PRIMs
        {
            set { AddClasses(value); }
        }
    }
}