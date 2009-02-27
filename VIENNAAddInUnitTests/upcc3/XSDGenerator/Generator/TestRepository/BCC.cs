namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BCC:UpccAttribute
    {
        public BCC(string name) : base(name)
        {
        }

        public override string GetStereotype()
        {
            return "BCC";
        }
    }
}