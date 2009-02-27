namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class CDT : DT
    {
        public CDT(string name) : base(name)
        {
        }

        public override string GetStereotype()
        {
            return "CDT";
        }
    }
}