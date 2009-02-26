namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class PRIM : EAElement
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