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
    public class ConnectorBuilder:RepositoryItemBuilder<ConnectorBuilder>
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