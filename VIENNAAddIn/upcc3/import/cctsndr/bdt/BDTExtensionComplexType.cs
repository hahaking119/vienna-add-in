using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    internal class BDTExtensionComplexType : BDTComplexType
    {
        public BDTExtensionComplexType(XmlSchemaComplexType complexType, BDTSchema schema) : base(complexType, schema)
        {
        }

        public override string ContentComponentXsdTypeName
        {
            get { return ((XmlSchemaSimpleContentExtension) ComplexType.ContentModel.Content).BaseTypeName.Name; }
        }

        public override void CreateBDT()
        {
            var extension = (XmlSchemaSimpleContentExtension) ComplexType.ContentModel.Content;
            IEnumerable<SUPSpec> sups = SpecifySUPs(extension.Attributes);
            CreateBDT(sups);
        }
    }
}