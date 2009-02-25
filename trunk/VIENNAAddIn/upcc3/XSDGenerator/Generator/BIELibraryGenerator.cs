using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class BIELibraryGenerator : AbstractLibraryGenerator
    {
        public BIELibraryGenerator(GenerationContext context)
            : base(context)
        {
        }

        public XmlSchema GenerateXSD(IBIELibrary library)
        {
            var schema = new XmlSchema();
            return schema;
        }
    }
}