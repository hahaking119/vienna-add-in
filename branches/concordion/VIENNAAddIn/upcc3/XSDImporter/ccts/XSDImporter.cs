using System.Xml;
using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.XSDImporter.ccts
{
    public static class XSDImporter
    {
        public static void ImportSchemas(ImporterContext context)
        {
            new BDTSchemaImporter(context).ImportXSD();
            BIESchemaImporter.ImportXSD(context);
            RootSchemaImporter.ImportXSD(context);
        }
    }
}