namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class SUP : UpccAttribute
    {
        public SUP(string name) : base(name)
        {
        }

        public override string GetStereotype()
        {
            return "SUP";
        }
    }
}