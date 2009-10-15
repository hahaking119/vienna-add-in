using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    public class BDTSchema
    {
        public IImporterContext Context { get; private set; }
        private readonly Dictionary<string, BDTXsdType> bdtTypes = new Dictionary<string, BDTXsdType>();

        public BDTSchema(IImporterContext context)
        {
            this.Context = context;
        }

        public void CreateBDTs()
        {
            foreach (XmlSchemaObject currentElement in Context.BDTSchema.Items)
            {
                if (currentElement is XmlSchemaComplexType)
                {
                    var bdtType = new BDTComplexType((XmlSchemaComplexType)currentElement, this);
                    bdtTypes[bdtType.XsdTypeName] = bdtType;
                }
                else if (currentElement is XmlSchemaSimpleType)
                {
                    var bdtType = new BDTSimpleType((XmlSchemaSimpleType)currentElement, this);
                    bdtTypes[bdtType.XsdTypeName] = bdtType;
                }
            }
            foreach (var bdtType in bdtTypes.Values)
            {
                bdtType.CreateBDT();
            }

        }

        public string CCTSNamespacePrefix
        {
            get
            {
                XmlQualifiedName[] qualifiedNames = Context.BDTSchema.Namespaces.ToArray();
                for (int i = 0; i < qualifiedNames.Length; i++)
                {
                    if (qualifiedNames[i].Namespace == NDR.CCTSNamespace)
                    {
                        return qualifiedNames[i].Name;
                    }
                }
                throw new Exception("CCTS namespace not defined: " + NDR.CCTSNamespace);
            }
        }
    }
}