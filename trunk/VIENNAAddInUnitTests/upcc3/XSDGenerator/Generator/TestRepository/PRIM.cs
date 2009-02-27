namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class PRIM : UpccClass
    {
        public PRIM(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "PRIM"; }
        }
    }
}