using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public interface ILibraryGenerator<T>
    {
        XmlSchema GenerateXSD(T library);
    }

    public abstract class AbstractLibraryGenerator<T> : ILibraryGenerator<T>
    {
        protected AbstractLibraryGenerator(GenerationContext context)
        {
            Context = context;
        }

        protected GenerationContext Context { get; private set; }

        public abstract XmlSchema GenerateXSD(T library);
    }
}