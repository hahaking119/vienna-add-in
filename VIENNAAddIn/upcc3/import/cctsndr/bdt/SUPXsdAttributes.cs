using System.Xml.Schema;
using CctsRepository;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    public class SUPXsdAttributes
    {
        private readonly XmlSchemaObjectCollection xsdAttributes;

        public SUPXsdAttributes(XmlSchemaObjectCollection xsdAttributes)
        {
            this.xsdAttributes = xsdAttributes;
        }

        private XmlSchemaAttribute FindSUPAttribute(BDTSupplementaryComponentSpec supSpec)
        {
            string attributeName = GetXsdAttributeNameFromSUPSpec(supSpec);
            foreach (XmlSchemaAttribute attribute in xsdAttributes)
            {
                if (attribute.Name == attributeName)
                {
                    return attribute;
                }
            }
            return null;
        }

        private static string GetXsdAttributeNameFromSUPSpec(BDTSupplementaryComponentSpec supSpec)
        {
            return supSpec.Name + supSpec.BasicType.Name;
        }

        public bool IsProhibited(BDTSupplementaryComponentSpec supSpec)
        {
            XmlSchemaAttribute supAttribute = FindSUPAttribute(supSpec);
            return supAttribute != null && supAttribute.Use == XmlSchemaUse.Prohibited;
        }

        public void ApplyRestrictions(BDTSupplementaryComponentSpec supSpec)
        {
        }
    }
}