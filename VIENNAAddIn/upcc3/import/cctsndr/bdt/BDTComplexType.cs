using System;
using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    public class BDTComplexType : BDTXsdType
    {
        public BDTComplexType(XmlSchemaComplexType complexType, BDTSchema schema) : base(complexType, schema)
        {
        }

        private XmlSchemaComplexType ComplexType
        {
            get { return (XmlSchemaComplexType) XsdType; }
        }

        private IEnumerable<SUPSpec> SpecifySUPs(XmlSchemaObjectCollection xsdAttributes, string conPrimName)
        {
            foreach (XmlSchemaAttribute attribute in xsdAttributes)
            {
                yield return new SUPSpec
                             {
                                 Name = attribute.Name.Replace(conPrimName, string.Empty),
                                 BasicType = FindPRIM(NDR.ConvertXsdTypeNameToBasicTypeName(attribute.SchemaTypeName.ToString().Replace("http://www.w3.org/2001/XMLSchema", string.Empty)))
                             };
            }
        }

        public override void CreateBDT()
        {
            var simpleContent = (XmlSchemaSimpleContentExtension) ComplexType.ContentModel.Content;
            String conPrimName = NDR.ConvertXsdTypeNameToBasicTypeName(simpleContent.BaseTypeName.Name);
            IEnumerable<SUPSpec> sups = SpecifySUPs(simpleContent.Attributes, conPrimName);
            CreateBDT(conPrimName, sups);
        }
    }
}