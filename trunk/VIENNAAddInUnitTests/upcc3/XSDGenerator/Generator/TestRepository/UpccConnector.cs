namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class UpccConnector : UpccElement
    {
        private readonly Path pathToSupplier;

        protected UpccConnector(Path pathToSupplier) : base(null)
        {
            this.pathToSupplier = pathToSupplier;
        }

        public Path GetPathToSupplier()
        {
            return pathToSupplier;
        }
    }
}