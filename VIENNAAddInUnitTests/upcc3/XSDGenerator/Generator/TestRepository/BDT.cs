using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class BDT : DT
    {
        public Path BasedOn
        {
            set { AddConnector(new BasedOnDependency {PathToSupplier = value}); }
        }

        public override string GetStereotype()
        {
            return "BDT";
        }
    }
}