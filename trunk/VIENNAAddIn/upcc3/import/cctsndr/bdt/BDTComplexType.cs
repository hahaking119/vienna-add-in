using System.Collections.Generic;
using System.Xml.Schema;
using CctsRepository;
using VIENNAAddIn.upcc3.export.cctsndr;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    public abstract class BDTComplexType : BDTXsdType
    {
        protected BDTComplexType(XmlSchemaComplexType complexType, BDTSchema schema) : base(complexType, schema)
        {
        }

        protected XmlSchemaComplexType ComplexType
        {
            get { return (XmlSchemaComplexType) XsdType; }
        }

        protected IEnumerable<BDTSupplementaryComponentSpec> SpecifySUPs(XmlSchemaObjectCollection xsdAttributes)
        {
            foreach (XmlSchemaAttribute attribute in xsdAttributes)
            {
                string basicTypeName = NDR.ConvertXsdTypeNameToBasicTypeName(attribute.SchemaTypeName.Name);
                yield return new BDTSupplementaryComponentSpec
                             {
                                 Name = attribute.Name.Minus(basicTypeName),
                                 BasicType = FindPRIM(basicTypeName)
                             };
            }
        }
    }
}