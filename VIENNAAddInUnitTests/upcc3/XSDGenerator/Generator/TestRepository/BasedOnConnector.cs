namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BasedOnConnector : EAConnector
    {
        public BasedOnConnector(Path basedOnType) : base(basedOnType)
        {
        }

        public override string Stereotype
        {
            get { return "basedOn"; }
        }
    }
}