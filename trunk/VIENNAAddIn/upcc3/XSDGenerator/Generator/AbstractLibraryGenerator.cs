namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public abstract class AbstractLibraryGenerator
    {
        protected AbstractLibraryGenerator(GenerationContext context)
        {
            Context = context;
        }

        protected GenerationContext Context { get; private set; }
    }
}