using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class BDTLibraryGenerator : AbstractLibraryGenerator
    {
        public BDTLibraryGenerator(GenerationContext context)
            : base(context)
        {
        }

        public XmlSchema GenerateXSD(IBDTLibrary library)
        {
            var schema = new XmlSchema();
            return schema;
        }
    }
}