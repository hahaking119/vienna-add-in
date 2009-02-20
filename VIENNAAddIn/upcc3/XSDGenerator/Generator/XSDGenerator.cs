using System;
using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class XSDGenerator
    {
        private readonly CDTLibraryGenerator cdtLibraryGenerator;
        private readonly GenerationContext context;

        public XSDGenerator(ICCRepository repository, bool annotate)
        {
            context = new GenerationContext(repository, annotate);
            cdtLibraryGenerator = new CDTLibraryGenerator(context);
        }

        public XmlSchema[] GenerateSchemas()
        {
            context.Repository.TraverseLibrariesDepthFirst(GenerateLibrarySchema);
            return context.Schemas.ToArray();
        }

        private void GenerateLibrarySchema(IBusinessLibrary library)
        {
            Console.WriteLine("Processing <<{0}>> {1}.", library.Type,
                              library.Name);
            switch (library.Type)
            {
                case BusinessLibraryType.CDTLibrary:
                    context.AddSchema(cdtLibraryGenerator.GenerateXSD((ICDTLibrary) library));
                    break;
                case BusinessLibraryType.bLibrary:
                case BusinessLibraryType.BDTLibrary:
                case BusinessLibraryType.BIELibrary:
                case BusinessLibraryType.CCLibrary:
                case BusinessLibraryType.DOCLibrary:
                case BusinessLibraryType.ENUMLibrary:
                case BusinessLibraryType.PRIMLibrary:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("unknown library type " + library.Type);
            }
        }
    }

    public class GenerationContext
    {
        private readonly Dictionary<string, NamespacePrefixFactory> namespacePrefixFactories =
            new Dictionary<string, NamespacePrefixFactory>();

        private readonly List<XmlSchema> schemas = new List<XmlSchema>();

        public GenerationContext(ICCRepository repository, bool annotate)
        {
            Repository = repository;
            Annotate = annotate;
        }

        public ICCRepository Repository { get; private set; }
        public bool Annotate { get; private set; }

        public List<XmlSchema> Schemas
        {
            get { return schemas; }
        }


        public void AddSchema(XmlSchema schema)
        {
            Schemas.Add(schema);
        }

        public string GetNextNamespacePrefix(string prefix)
        {
            if (!namespacePrefixFactories.ContainsKey(prefix))
            {
                namespacePrefixFactories[prefix] = new NamespacePrefixFactory(prefix);
            }
            return namespacePrefixFactories[prefix].GetNextNamespacePrefix();
        }

        public void appendWarnMessage(string message, string packageName)
        {
            throw new NotImplementedException();
        }
    }

    public class NamespacePrefixFactory
    {
        private readonly string prefix;
        private int counter;

        public NamespacePrefixFactory(string prefix)
        {
            this.prefix = prefix;
        }

        public string GetNextNamespacePrefix()
        {
            return prefix + (++counter);
        }
    }
}