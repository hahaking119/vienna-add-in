// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class ConnectorBuilder : RepositoryItemBuilder<ConnectorBuilder>
    {
        private readonly Path pathToSupplier;
        private string lowerBound = "1";
        private string upperBound = "1";

        public ConnectorBuilder(string name, string stereotype, Path pathToSupplier) : base(name, stereotype)
        {
            this.pathToSupplier = pathToSupplier;
        }

        public Path GetPathToSupplier()
        {
            return pathToSupplier;
        }

        public ConnectorBuilder LowerBound(string lowerBound)
        {
            this.lowerBound = lowerBound;
            return this;
        }

        public ConnectorBuilder UpperBound(string upperBound)
        {
            this.upperBound = upperBound;
            return this;
        }

        public string GetLowerBound()
        {
            return lowerBound;
        }

        public string GetUpperBound()
        {
            return upperBound;
        }
    }
}