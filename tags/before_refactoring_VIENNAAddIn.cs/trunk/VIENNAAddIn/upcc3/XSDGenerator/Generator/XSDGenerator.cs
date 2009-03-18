// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class XSDGenerator
    {
        private readonly GenerationContext context;
        private readonly TypeBasedGenerator generator = new TypeBasedGenerator();

        public XSDGenerator(ICCRepository repository, bool annotate)
        {
            context = new GenerationContext(repository, annotate);
            generator.AddGenerator(new BDTLibraryGenerator(context));
            generator.AddGenerator(new BIELibraryGenerator(context));
            generator.AddGenerator(new CCLibraryGenerator(context));
            generator.AddGenerator(new DOCLibraryGenerator(context));
            generator.AddGenerator(new ENUMLibraryGenerator(context));
            generator.AddGenerator(new PRIMLibraryGenerator(context));
            generator.AddGenerator(new CDTLibraryGenerator(context));
        }

        public IEnumerable<XmlSchema> GenerateSchemas()
        {
            foreach (IBusinessLibrary library in context.Repository.AllLibraries())
            {
                Console.WriteLine("Processing {0}.", library.Name);
                XmlSchema schema = generator.Execute(library);
                if (schema != null)
                {
                    yield return schema;
                }
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