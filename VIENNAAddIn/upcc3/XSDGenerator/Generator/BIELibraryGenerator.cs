using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class BIELibraryGenerator : AbstractLibraryGenerator<IBIELibrary>
    {
        public BIELibraryGenerator(GenerationContext context)
            : base(context)
        {
        }

        public override XmlSchema GenerateXSD(IBIELibrary library)
        {
            var schema = new XmlSchema();
            return schema;
        }
    }
}