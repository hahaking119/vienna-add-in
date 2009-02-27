namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class UpccBLibrary : UpccPackage
    {
        public UpccBLibrary(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "bLibrary"; }
        }
    }
}