using System;
using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.import.cctsndr
{
    public class BDTComplexType : BDTXsdType
    {
        private readonly XmlSchemaComplexType complexType;

        public BDTComplexType(IImporterContext context, XmlSchemaComplexType complexType, BDTSchema schema) : base(complexType, schema)
        {
            this.complexType = complexType;
        }

        private IEnumerable<SUPSpec> SpecifySUPs(XmlSchemaObjectCollection xsdAttributes, string conPrimName)
        {
            foreach (XmlSchemaAttribute attribute in xsdAttributes)
            {
                yield return SpecifySUP(attribute, conPrimName,
                                        NDR.ConvertXsdTypeNameToBasicTypeName(attribute.SchemaTypeName.ToString().Replace("http://www.w3.org/2001/XMLSchema", string.Empty)));
            }
        }

        private SUPSpec SpecifySUP(XmlSchemaAttribute attribute, string conPrimName, string primName)
        {
            return new SUPSpec
                   {
                       Name = attribute.Name.Replace(conPrimName, string.Empty),
                       BasicType = FindPRIM(primName)
                   };
        }

        public override void CreateBDT()
        {
            var simpleContent = (XmlSchemaSimpleContentExtension) complexType.ContentModel.Content;
            String conPrimName = NDR.ConvertXsdTypeNameToBasicTypeName(simpleContent.BaseTypeName.Name);
            var sups = SpecifySUPs(simpleContent.Attributes, conPrimName);
            CreateBDT(conPrimName, sups);
        }
    }
}