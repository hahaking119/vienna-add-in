using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.export.cctsndr;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    public abstract class BDTXsdType
    {
        protected readonly BDTSchema bdtSchema;

        protected BDTXsdType(XmlSchemaType xsdType, BDTSchema bdtSchema)
        {
            XsdType = xsdType;
            this.bdtSchema = bdtSchema;
        }

        protected XmlSchemaType XsdType { get; private set; }

        public IImporterContext Context
        {
            get { return bdtSchema.Context; }
        }

        public string XsdTypeName
        {
            get { return XsdType.Name; }
        }

        public void CreateBDT()
        {
            string bdtName = GetBdtNameFromXsdType();
            ICDT cdt = Context.CDTLibrary.ElementByName(GetDataTypeTerm());
            var bdtSpec = new BDTSpec
            {
                BasedOn = cdt,
                Name = bdtName,
                CON = SpecifyCON(),
                SUPs = new List<SUPSpec>(SpecifySUPs()),
            };           

            BDT = Context.BDTLibrary.CreateElement(bdtSpec);
        }

        protected abstract IEnumerable<SUPSpec> SpecifySUPs();

        protected IPRIM FindPRIM(string primName)
        {
            IPRIM prim = Context.PRIMLibrary.ElementByName(primName);
            if (prim == null)
            {
                throw new Exception(String.Format("PRIM not found: {0}", primName));
            }
            return prim;
        }

        protected IBDT GetBDTByXsdTypeName(string typeName)
        {
            return bdtSchema.GetBDTByXsdTypeName(typeName);
        }

        public IBDT BDT { get; private set; }
        public abstract string ContentComponentXsdTypeName { get; }

        private string GetDataTypeTerm()
        {
            string xsdTypeNameWithoutGuid = XsdType.Name.Substring(0, XsdType.Name.LastIndexOf('_'));

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

            if (XsdType.Annotation != null)
            {
                XmlSchemaObjectCollection annotationItems = XsdType.Annotation.Items;
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

        protected virtual CONSpec SpecifyCON()
        {
            return new CONSpec
                   {
                       BasicType = FindPRIM(NDR.ConvertXsdTypeNameToBasicTypeName(ContentComponentXsdTypeName))
                   };
        }
    }
}