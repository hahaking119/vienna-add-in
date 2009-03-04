using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class ABIE : UpccClass
    {
        public Path BasedOn
        {
            set { AddConnector(new BasedOnDependency { PathToSupplier = value }); }
        }

        public List<BBIE> BBIEs
        {
            set { AddAttributes(value); }
        }

        public List<ASBIE> ASBIEs
        {
            set { AddConnectors(value); }
        }

        public Path IsEquivalentTo
        {
            set { AddConnector(new IsEquivalentToDependency {PathToSupplier = value}); }
        }

        public override string GetStereotype()
        {
            return "ABIE";
        }
    }
}