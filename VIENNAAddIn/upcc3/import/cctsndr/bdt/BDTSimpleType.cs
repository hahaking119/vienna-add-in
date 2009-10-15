using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    internal class BDTSimpleType : BDTXsdType
    {
        public BDTSimpleType(XmlSchemaSimpleType simpleType, BDTSchema bdtSchema) : base(simpleType, bdtSchema)
        {
        }

        private XmlSchemaSimpleType SimpleType
        {
            get { return (XmlSchemaSimpleType) XsdType; }
        }

        public override void CreateBDT()
        {
            var simpleContent = (XmlSchemaSimpleTypeRestriction) SimpleType.Content;
            string conPrimName = NDR.ConvertXsdTypeNameToBasicTypeName(simpleContent.BaseTypeName.Name);
            CreateBDT(conPrimName, new List<SUPSpec>());
        }
    }
}