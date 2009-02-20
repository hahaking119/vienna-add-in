using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class CCLibraryGenerator : AbstractLibraryGenerator
    {
        public CCLibraryGenerator(GenerationContext context)
            : base(BusinessLibraryType.CCLibrary, context)
        {
        }
    }
}