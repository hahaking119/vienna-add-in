using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class ACC : UpccClass
    {
        public ACC(string name) : base(name)
        {
        }

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
            set { AddConnector(new IsEquivalentToDependency(value)); }
        }

        public override string GetStereotype()
        {
            return "ACC";
        }
    }

    internal class IsEquivalentToDependency : UpccConnector
    {
        public IsEquivalentToDependency(Path acc) : base(acc)
        {
        }

        public override string GetStereotype()
        {
            return "isEquivalentTo";
        }
    }
}