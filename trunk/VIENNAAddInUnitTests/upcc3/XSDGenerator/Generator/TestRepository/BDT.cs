namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BDT : EAElement
    {
        public BDT(string name) : base(name)
        {
        }

        public Path BasedOn
        {
            set
            {
                connectors.Add(new BasedOnConnector(value));
            }
        }

        public override string Stereotype
        {
            get { return "BDT"; }
        }
    }
}