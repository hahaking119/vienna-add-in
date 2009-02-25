using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class CCLibraryGenerator : AbstractLibraryGenerator
    {
        public CCLibraryGenerator(GenerationContext context)
            : base(context)
        {
        }

        public XmlSchema GenerateXSD(ICCLibrary library)
        {
            var schema = new XmlSchema();
            return schema;
        }
    }
}