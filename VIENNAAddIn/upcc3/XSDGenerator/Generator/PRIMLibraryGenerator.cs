using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class PRIMLibraryGenerator : AbstractLibraryGenerator<IPRIMLibrary>
    {
        public PRIMLibraryGenerator(GenerationContext context)
            : base(context)
        {
        }

        public override XmlSchema GenerateXSD(IPRIMLibrary library)
        {
            var schema = new XmlSchema();
            return schema;
        }
    }
}