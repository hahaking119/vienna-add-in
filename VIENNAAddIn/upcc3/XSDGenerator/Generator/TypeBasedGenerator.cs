using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    internal class TypeBasedGenerator : TypeBasedSelector<XmlSchema>
    {
        public TypeBasedGenerator()
        {
            Default(arg => null);
        }

        public void AddGenerator<T>(ILibraryGenerator<T> generator)
        {
            Case<T>(l => generator.GenerateXSD(l));
        }
    }
}