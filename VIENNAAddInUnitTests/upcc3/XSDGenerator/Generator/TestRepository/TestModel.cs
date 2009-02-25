using System.Collections.Generic;
using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class TestModel : TestLibrary
    {
        public override string Stereotype
        {
            get { throw new System.NotImplementedException(); }
        }

        public override TestRepositoryElement AddBLibrary()
        {
            return AddLibrary(new TestBLibrary());
        }
    }
}