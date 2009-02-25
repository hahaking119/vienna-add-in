using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class ENUMLibraryGenerator : AbstractLibraryGenerator
    {
        public ENUMLibraryGenerator(GenerationContext context)
            : base(context)
        {
        }

        public XmlSchema GenerateXSD(IENUMLibrary library)
        {
            var schema = new XmlSchema();
            return schema;
        }
    }
}