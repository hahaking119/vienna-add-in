namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class TestCDTLibrary : TestLibrary
    {
        public override string Stereotype
        {
            get { return "CDTLibrary"; }
        }

        public override TestRepositoryElement AddCDT()
        {
            return AddElement(new TestCDT());
        }
    }
}