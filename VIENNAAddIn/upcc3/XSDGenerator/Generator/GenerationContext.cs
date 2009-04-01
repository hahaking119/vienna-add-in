using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    ///<summary>
    ///</summary>
    public class GenerationContext
    {
        private readonly List<XmlSchema> schemas = new List<XmlSchema>();

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="targetNamespace"></param>
        ///<param name="namespacePrefix"></param>
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
        public List<XmlSchema> Schemas
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
        public void AddSchema(XmlSchema schema)
        {
            Schemas.Add(schema);
        }
    }
}