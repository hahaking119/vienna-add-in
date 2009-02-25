namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class TestBLibrary : TestLibrary
    {
        public override string Stereotype
        {
            get { return "bLibrary"; }
        }

        public override TestRepositoryElement AddBLibrary()
        {
            return AddLibrary(new TestBLibrary());
        }

        public override TestRepositoryElement AddCDTLibrary()
        {
            return AddLibrary(new TestCDTLibrary());
        }

        public override TestRepositoryElement AddPRIMLibrary()
        {
            return AddLibrary(new TestPRIMLibrary());
        }
    }
}