namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class SUP : EAAttribute
    {
        public SUP(string name) : base(name)
        {
        }

        public override string Stereotype
        {
            get { return "SUP"; }
        }
    }
}