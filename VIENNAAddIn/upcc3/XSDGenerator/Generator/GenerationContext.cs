using System;
using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    ///<summary>
    ///</summary>
    public class GenerationContext
    {
        private readonly int progress;
        private readonly List<SchemaInfo> schemas = new List<SchemaInfo>();

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="targetNamespace"></param>
        ///<param name="namespacePrefix"></param>
        ///<param name="annotate"></param>
        ///<param name="outputDirectory"></param>
        ///<param name="docLibrary"></param>
        ///<param name="rootElements"></param>
        public GenerationContext(ICCRepository repository, string targetNamespace, string namespacePrefix, bool annotate,
                                 string outputDirectory, IDOCLibrary docLibrary, IList<IABIE> rootElements)
        {
            Repository = repository;
            TargetNamespace = targetNamespace;
            NamespacePrefix = namespacePrefix;
            Annotate = annotate;
            OutputDirectory = outputDirectory;
            DocLibrary = docLibrary;
            RootElements = rootElements;
            progress = 100/(2 + (rootElements != null ? rootElements.Count : 0));
        }

        ///<summary>
        ///</summary>
        public ICCRepository Repository { get; private set; }

        ///<summary>
        ///</summary>
        public string TargetNamespace { get; private set; }

        ///<summary>
        ///</summary>
        public List<SchemaInfo> Schemas
        {
            get { return schemas; }
        }

        ///<summary>
        ///</summary>
        public string NamespacePrefix { get; private set; }

        ///<summary>
        ///</summary>
        public bool Annotate { get; private set; }

        ///<summary>
        ///</summary>
        public string OutputDirectory { get; private set; }

        ///<summary>
        ///</summary>
        public IDOCLibrary DocLibrary { get; private set; }

        ///<summary>
        ///</summary>
        public IList<IABIE> RootElements { get; private set; }

        ///<summary>
        ///</summary>
        ///<param name="schema"></param>
        ///<param name="fileName"></param>
        public void AddSchema(XmlSchema schema, string fileName)
        {
            Schemas.Add(new SchemaInfo(schema, fileName));
            if (SchemaAdded != null)
            {
                SchemaAdded(this, new SchemaAddedEventArgs(fileName, progress));
            }
        }

        public event EventHandler<SchemaAddedEventArgs> SchemaAdded;
    }

    public class GenerationMessage
    {
    }

    public class SchemaAddedEventArgs : EventArgs
    {
        public SchemaAddedEventArgs(string fileName, int progress)
        {
            FileName = fileName;
            Progress = progress;
        }

        public string FileName { get; private set; }
        public int Progress { get; private set; }
    }

    ///<summary>
    ///</summary>
    public class SchemaInfo
    {
        ///<summary>
        ///</summary>
        ///<param name="schema"></param>
        ///<param name="fileName"></param>
        public SchemaInfo(XmlSchema schema, string fileName)
        {
            Schema = schema;
            FileName = fileName;
        }

        ///<summary>
        ///</summary>
        public XmlSchema Schema { get; private set; }

        ///<summary>
        ///</summary>
        public string FileName { get; private set; }
    }
}