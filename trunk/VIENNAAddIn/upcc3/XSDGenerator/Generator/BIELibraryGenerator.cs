using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class BIELibraryGenerator : AbstractLibraryGenerator
    {
        public BIELibraryGenerator(GenerationContext context)
            : base(BusinessLibraryType.BIELibrary, context)
        {
        }
    }
}