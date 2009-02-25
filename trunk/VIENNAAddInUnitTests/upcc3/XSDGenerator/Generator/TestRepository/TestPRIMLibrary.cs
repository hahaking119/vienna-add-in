namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class TestPRIMLibrary : TestLibrary
    {
        public override string Stereotype
        {
            get { return "PRIMLibrary"; }
        }

        public override TestRepositoryElement AddPRIM()
        {
            return AddElement(new TestPRIM());
        }
    }
}