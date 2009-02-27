namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BDT : DT
    {
        public BDT(string name) : base(name)
        {
        }

        public Path BasedOn
        {
            set
            {
                AddConnector(new BasedOnDependency(value));
            }
        }

        public override string GetStereotype()
        {
            return "BDT";
        }
    }
}