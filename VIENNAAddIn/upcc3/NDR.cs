using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3
{
    public static class NDR
    {
        private const string Identification = "Identification";
        private const string Identifier = "Identifier";
        private const string Indication = "Indication";
        private const string Indicator = "Indicator";
        private const string Text = "Text";

        public static string GenerateBCCName(IBCC bcc)
        {
            return GenerateBCCOrBBIEName(bcc.Name, bcc.Type.Name);
        }

        public static string GenerateBBIEName(IBBIE bbie)
        {
            return GenerateBCCOrBBIEName(bbie.Name, bbie.Type.Name);
        }

        private static string GenerateBCCOrBBIEName(string propertyTerm, string representationTerm)
        {
            if ((propertyTerm.EndsWith(Identification)) && (representationTerm.Equals(Identifier)))
            {
                return propertyTerm.Remove(propertyTerm.Length - Identification.Length) + Identifier;
            }

            if ((propertyTerm.EndsWith(Indication)) && (representationTerm.Equals(Indicator)))
            {
                return propertyTerm.Remove(propertyTerm.Length - Indication.Length) + Indicator;
            }

            if (representationTerm.Equals(Text))
            {
                return propertyTerm;
            }

            return propertyTerm + representationTerm;
        }

        public static string GenerateASCCName(IASCC ascc)
        {
            return ascc.Name + ascc.AssociatedElement.Name;
        }

        public static string GenerateASBIEName(IASBIE asbie)
        {
            return asbie.Name + asbie.AssociatedElement.Name;
        }
    }
}