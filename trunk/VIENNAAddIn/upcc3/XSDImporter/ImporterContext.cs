using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;

namespace VIENNAAddIn.upcc3.XSDImporter
{
    class ImporterContext
    {
        private readonly List<SchemaInfo> schemas = new List<SchemaInfo>();

        ///<summary>
        ///</summary>
        public ICCRepository Repository { get; private set; }

        ///<summary>
        ///</summary>
        public List<SchemaInfo> Schemas
        {
            get { return schemas; }
        }
    }
}
