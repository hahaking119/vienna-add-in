using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    ///<summary>
    ///</summary>
    public class GenerationContext
    {
        private readonly List<SchemaInfo> schemas = new List<SchemaInfo>();

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="targetNamespace"></param>
        ///<param name="namespacePrefix"></param>
        ///<param name="annotate"></param>
        public GenerationContext(ICCRepository repository, string targetNamespace, string namespacePrefix, bool annotate)
        {
            Repository = repository;
            TargetNamespace = targetNamespace;
            NamespacePrefix = namespacePrefix;
            Annotate = annotate;
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
        ///<param name="schema"></param>
        ///<param name="fileName"></param>
        public void AddSchema(XmlSchema schema, string fileName)
        {
            Schemas.Add(new SchemaInfo(schema, fileName));
        }
    }

    ///<summary>
    ///</summary>
    public class SchemaInfo
    {
        ///<summary>
        ///</summary>
        public XmlSchema Schema { get; private set; }
        ///<summary>
        ///</summary>
        public string FileName { get; private set; }

        ///<summary>
        ///</summary>
        ///<param name="schema"></param>
        ///<param name="fileName"></param>
        public SchemaInfo(XmlSchema schema, string fileName)
        {
            Schema = schema;
            FileName = fileName;
        }
    }
}