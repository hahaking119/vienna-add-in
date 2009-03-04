using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class Stereotype
    {
        public const string ABIE = "ABIE";
        public const string ACC = "ACC";
        public const string ASBIE = "ASBIE";
        public const string ASCC = "ASCC";
        public const string BasedOn = "basedOn";
        public const string BBIE = "BBIE";
        public const string BCC = "BCC";
        public const string BDT = "BDT";
        public const string CDT = "CDT";
        public const string CON = "CON";
        public const string IsEquivalentTo = "isEquivalentTo";
        public const string SUP = "SUP";

        public static bool IsIsEquivalentTo(this Connector con)
        {
            return con.Stereotype == IsEquivalentTo;
        }

        public static bool IsBasedOn(this Connector con)
        {
            return con.Stereotype == BasedOn;
        }

        public static bool IsASCC(this Connector con)
        {
            return con.Stereotype == ASCC;
        }

        public static bool IsASBIE(this Connector con)
        {
            return con.Stereotype == ASBIE;
        }

        public static bool IsCON(this Attribute attribute)
        {
            return attribute.Stereotype == CON;
        }

        public static bool IsSUP(this Attribute attribute)
        {
            return attribute.Stereotype == SUP;
        }
    }
}