namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BLibrary : Library
    {
        public BLibrary(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "bLibrary"; }
        }
    }
}