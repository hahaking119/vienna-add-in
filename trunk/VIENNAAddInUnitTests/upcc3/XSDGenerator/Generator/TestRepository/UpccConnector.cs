namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class UpccConnector : UpccElement
    {
        public Path PathToSupplier { get; private set; }

        protected UpccConnector(Path pathToSupplier) : base(null)
        {
            PathToSupplier = pathToSupplier;
        }
    }
}