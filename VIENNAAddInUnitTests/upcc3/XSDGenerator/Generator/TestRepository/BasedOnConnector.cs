namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BasedOnConnector : UpccConnector
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