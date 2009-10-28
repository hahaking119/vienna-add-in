using System.Collections.Generic;
using System.Xml.Schema;
using UPCCRepositoryInterface;
using VIENNAAddIn.upcc3.export.cctsndr;

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

        protected override IEnumerable<SUPSpec> SpecifySUPs()
        {
            var extension = (XmlSchemaSimpleContentExtension) ComplexType.ContentModel.Content;
            foreach (XmlSchemaAttribute attribute in extension.Attributes)
            {
                string basicTypeName = NDR.ConvertXsdTypeNameToBasicTypeName(attribute.SchemaTypeName.Name);
                yield return new SUPSpec
                                 {
                                     Name = attribute.Name.Minus(basicTypeName),
                                     BasicType = FindPRIM(basicTypeName)
                                 };
            }
        }

        protected override CONSpec SpecifyCON()
        {
            return new CONSpec
                       {
                           BasicType =
                               FindPRIM(NDR.ConvertXsdTypeNameToBasicTypeName(ContentComponentXsdTypeName))
                       };
        }
    }
}