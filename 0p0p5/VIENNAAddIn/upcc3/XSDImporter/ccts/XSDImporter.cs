using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VIENNAAddIn.upcc3.XSDImporter.ccts
{
    public class XSDImporter
    {
        public static ImporterContext ImportSchemas (ImporterContext context)
        {
            BDTSchemaImporter.ImportXSD(context);
            BIESchemaImporter.ImportXSD(context);
            RootSchemaImporter.ImportXSD(context);

            return context;
        }
    }
}
