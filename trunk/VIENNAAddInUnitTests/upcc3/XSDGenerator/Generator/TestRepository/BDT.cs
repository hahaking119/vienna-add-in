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
                Connectors.Add(new BasedOnConnector(value));
            }
        }

        public override string Stereotype
        {
            get { return "BDT"; }
        }
    }
}