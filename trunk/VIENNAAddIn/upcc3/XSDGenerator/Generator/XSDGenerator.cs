using System;
using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class XSDGenerator
    {
        private readonly BDTLibraryGenerator bdtLibraryGenerator;
        private readonly BIELibraryGenerator bieLibraryGenerator;
        private readonly CCLibraryGenerator ccLibraryGenerator;
        private readonly DOCLibraryGenerator docLibraryGenerator;
        private readonly ENUMLibraryGenerator enumLibraryGenerator;
        private readonly PRIMLibraryGenerator primLibraryGenerator;
        private readonly CDTLibraryGenerator cdtLibraryGenerator;
        private readonly GenerationContext context;

        public XSDGenerator(ICCRepository repository, bool annotate)
        {
            context = new GenerationContext(repository, annotate);
            bdtLibraryGenerator = new BDTLibraryGenerator(context);
            bieLibraryGenerator = new BIELibraryGenerator(context);
            ccLibraryGenerator = new CCLibraryGenerator(context);
            docLibraryGenerator = new DOCLibraryGenerator(context);
            enumLibraryGenerator = new ENUMLibraryGenerator(context);
            primLibraryGenerator = new PRIMLibraryGenerator(context);
            cdtLibraryGenerator = new CDTLibraryGenerator(context);
        }

        public IEnumerable<XmlSchema> GenerateSchemas()
        {
            foreach (IBusinessLibrary library in context.Repository.Libraries)
            {
                XmlSchema schema = GenerateLibrarySchema(library);
                if (schema != null)
                {
                    yield return schema;
                }
            }
        }

        private XmlSchema GenerateLibrarySchema(IBusinessLibrary library)
        {
            Console.WriteLine("Processing <<{0}>> {1}.", library.Type,
                              library.Name);
            switch (library.Type)
            {
                case BusinessLibraryType.CDTLibrary:
                    return cdtLibraryGenerator.GenerateXSD((ICDTLibrary) library);
                case BusinessLibraryType.BIELibrary:
                    return bieLibraryGenerator.GenerateXSD((IBIELibrary)library);
                case BusinessLibraryType.CCLibrary:
                    return ccLibraryGenerator.GenerateXSD((ICCLibrary)library);
                case BusinessLibraryType.DOCLibrary:
                    return docLibraryGenerator.GenerateXSD((IDOCLibrary)library);
                case BusinessLibraryType.ENUMLibrary:
                    return enumLibraryGenerator.GenerateXSD((IENUMLibrary)library);
                case BusinessLibraryType.BDTLibrary:
                    return bdtLibraryGenerator.GenerateXSD((IBDTLibrary)library);
                case BusinessLibraryType.PRIMLibrary:
                    return primLibraryGenerator.GenerateXSD((IPRIMLibrary)library);
                case BusinessLibraryType.bLibrary:
                    return null;
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