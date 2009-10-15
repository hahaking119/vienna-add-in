using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.import.cctsndr
{
    internal class BDTSimpleType : BDTXsdType
    {
        private readonly XmlSchemaSimpleType simpleType;

        public BDTSimpleType(XmlSchemaSimpleType simpleType, BDTSchema bdtSchema) : base(simpleType, bdtSchema)
        {
            this.simpleType = simpleType;
        }

        public override void CreateBDT()
        {
            var simpleContent = (XmlSchemaSimpleTypeRestriction) SimpleType.Content;
            var conPrimName = NDR.ConvertXsdTypeNameToBasicTypeName(simpleContent.BaseTypeName.Name);
            CreateBDT(conPrimName, new List<SUPSpec>());
        }

        private XmlSchemaSimpleType SimpleType
        {
            get { return simpleType; }
        }
    }
}