using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class ConnectorBuilder:EAObjectBuilder<ConnectorBuilder>
    {
        private readonly Path pathToSupplier;

        public ConnectorBuilder(string name, string stereotype, Path pathToSupplier) : base(name, stereotype)
        {
            this.pathToSupplier = pathToSupplier;
        }

        public Path GetPathToSupplier()
        {
            return pathToSupplier;
        }
    }
}