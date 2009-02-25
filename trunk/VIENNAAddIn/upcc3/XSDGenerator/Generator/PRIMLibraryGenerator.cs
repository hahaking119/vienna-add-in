using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class PRIMLibraryGenerator : AbstractLibraryGenerator
    {
        public PRIMLibraryGenerator(GenerationContext context)
            : base(context)
        {
        }

        public XmlSchema GenerateXSD(IPRIMLibrary library)
        {
            var schema = new XmlSchema();
            return schema;
        }
    }
}