using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.export.cctsndr;

namespace VIENNAAddIn.upcc3.import.cctsndr
{
    public abstract class BDTXsdType
    {
        private readonly BDTSchema bdtSchema;
        private readonly XmlSchemaType xsdType;

        protected BDTXsdType(XmlSchemaType xsdType, BDTSchema bdtSchema)
        {
            this.xsdType = xsdType;
            this.bdtSchema = bdtSchema;
            XsdTypeName = xsdType.Name;
        }

        public IImporterContext Context
        {
            get { return bdtSchema.Context; }
        }

        public string XsdTypeName { get; private set; }
        public abstract void CreateBDT();

        protected IPRIM FindPRIM(string primName)
        {
            IPRIM prim = Context.PRIMLibrary.ElementByName(primName);
            if (prim == null)
            {
                throw new Exception(String.Format("PRIM not found: {0}", primName));
            }
            return prim;
        }

        private CONSpec SpecifyCON(string primName)
        {
            return new CONSpec
                   {
                       BasicType = FindPRIM(primName)
                   };
        }

        protected void CreateBDT(string conPrimName, IEnumerable<SUPSpec> sups)
        {
            string bdtName = GetBdtNameFromXsdType();
            ICDT cdt = Context.CDTLibrary.ElementByName(GetDataTypeTerm());
            var bdtSpec = new BDTSpec
                          {
                              BasedOn = cdt,
                              Name = bdtName,
                              CON = SpecifyCON(conPrimName),
                              SUPs = new List<SUPSpec>(sups),
                          };
            Context.BDTLibrary.CreateElement(bdtSpec);
        }

        private string GetDataTypeTerm()
        {
            string xsdTypeNameWithoutGuid = xsdType.Name.Substring(0, xsdType.Name.LastIndexOf('_'));

            string xsdTypeNameWithQualifiers = xsdTypeNameWithoutGuid.Minus("Type");

            var qualifierString = new StringBuilder();
            foreach (string qualifier in GetQualifiers())
            {
                qualifierString.Append(qualifier);
            }

            return xsdTypeNameWithQualifiers.Substring(qualifierString.Length);
        }

        private string GetBdtNameFromXsdType()
        {
            var qualifiersForClassName = new StringBuilder();
            foreach (string qualifier in GetQualifiers())
            {
                qualifiersForClassName.Append(qualifier).Append("_");
            }
            return qualifiersForClassName + GetDataTypeTerm();
        }

        private List<string> GetQualifiers()
        {
            string cctsNamespacePrefix = bdtSchema.CCTSNamespacePrefix;

            var qualifiers = new List<string>();

            if (xsdType.Annotation != null)
            {
                XmlSchemaObjectCollection annotationItems = xsdType.Annotation.Items;
                foreach (XmlSchemaObject item in annotationItems)
                {
                    if (item is XmlSchemaDocumentation)
                    {
                        XmlNode[] nodes = ((XmlSchemaDocumentation) item).Markup;
                        foreach (XmlNode node in nodes)
                        {
                            if (node is XmlElement)
                            {
                                if (node.Name == cctsNamespacePrefix + ":DataTypeQualifierTermName")
                                {
                                    string qualifier = node.InnerText;
                                    qualifiers.Add(qualifier);
                                }
                            }
                        }
                    }
                }
            }
            return qualifiers;
        }
    }
}