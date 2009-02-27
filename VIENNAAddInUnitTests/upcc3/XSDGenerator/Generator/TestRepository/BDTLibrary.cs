using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BDTLibrary : UpccPackage
    {
        public BDTLibrary(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "BDTLibrary"; }
        }

        public List<BDT> BDTs
        {
            set { AddClasses(value); }
        }
    }
}