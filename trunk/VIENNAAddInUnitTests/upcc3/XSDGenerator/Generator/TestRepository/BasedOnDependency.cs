namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BasedOnDependency : UpccConnector
    {
        public BasedOnDependency(Path basedOnType) : base(basedOnType)
        {
        }

        public override string GetStereotype()
        {
            return "basedOn";
        }
    }
}