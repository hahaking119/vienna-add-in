using System;
using EA;
using Attribute=EA.Attribute;

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
        public const string BDTLibrary = "BDTLibrary";
        public const string BIELibrary = "BIELibrary";
        public const string BLibrary = "bLibrary";
        public const string CCLibrary = "CCLibrary";
        public const string CDT = "CDT";
        public const string CDTLibrary = "CDTLibrary";
        public const string CON = "CON";
        public const string ENUMLibrary = "ENUMLibrary";
        public const string IsEquivalentTo = "isEquivalentTo";
        public const string PRIMLibrary = "PRIMLibrary";
        public const string SUP = "SUP";
        public const string PRIM = "PRIM";
        public const string ENUM = "ENUM";

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

        public static string GetStereotype<T>()
        {
            if (typeof(T) == typeof(IBLibrary))
            {
                return BLibrary;
            }
            if (typeof(T) == typeof(ICCLibrary))
            {
                return CCLibrary;
            }
            if (typeof(T) == typeof(ICDTLibrary))
            {
                return CDTLibrary;
            }
            if (typeof(T) == typeof(IBIELibrary))
            {
                return BIELibrary;
            }
            if (typeof(T) == typeof(IBDTLibrary))
            {
                return BDTLibrary;
            }
            if (typeof(T) == typeof(IPRIMLibrary))
            {
                return PRIMLibrary;
            }
            if (typeof(T) == typeof(IENUMLibrary))
            {
                return ENUMLibrary;
            }
            if (typeof(T) == typeof(ICDT))
            {
                return CDT;
            }
            if (typeof(T) == typeof(IACC))
            {
                return ACC;
            }
            if (typeof(T) == typeof(IASCC))
            {
                return ASCC;
            }
            if (typeof(T) == typeof(IBCC))
            {
                return BCC;
            }
            if (typeof(T) == typeof(IBDT))
            {
                return BDT;
            }
            if (typeof(T) == typeof(IABIE))
            {
                return ABIE;
            }
            if (typeof(T) == typeof(IBBIE))
            {
                return BBIE;
            }
            if (typeof(T) == typeof(IASBIE))
            {
                return ASBIE;
            }
            if (typeof(T) == typeof(IPRIM))
            {
                return PRIM;
            }
            if (typeof(T) == typeof(IENUM))
            {
                return ENUM;
            }
            if (typeof(T) == typeof(ISUP))
            {
                return SUP;
            }
            if (typeof(T) == typeof(ICON))
            {
                return CON;
            }
            return String.Empty;
        }
    }
}