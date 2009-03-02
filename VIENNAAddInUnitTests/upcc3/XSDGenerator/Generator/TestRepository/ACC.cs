using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class ACC : UpccClass
    {
        public List<BCC> BCCs
        {
            set { AddAttributes(value); }
        }

        public List<ASCC> ASCCs
        {
            set { AddConnectors(value); }
        }

        public Path IsEquivalentTo
        {
            set { AddConnector(new IsEquivalentToDependency {PathToSupplier = value}); }
        }

        public override string GetStereotype()
        {
            return "ACC";
        }
    }
}