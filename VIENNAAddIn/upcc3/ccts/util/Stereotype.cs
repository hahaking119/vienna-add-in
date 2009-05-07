// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.util
{
    ///<summary>
    /// Definition of stereotype strings for UPCC elements.
    ///</summary>
    public static class Stereotype
    {
#pragma warning disable 1591
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
        public const string DOCLibrary = "DOCLibrary";
        public const string BLibrary = "bLibrary";
        public const string CCLibrary = "CCLibrary";
        public const string BInformationV = "bInformationV";
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

        public static bool IsACC(this Element element)
        {
            return element.Stereotype == ACC;
        }

        public static bool IsCDT(this Element element)
        {
            return element.Stereotype == CDT;
        }

        public static bool IsABIE(this Element element)
        {
            return IsA(element, ABIE);
        }

        public static bool IsA(this Element element, string stereotype)
        {
            return element.Stereotype == stereotype;
        }

        public static bool IsBDT(this Element element)
        {
            return element.Stereotype == BDT;
        }

        public static bool IsPRIM(this Element element)
        {
            return element.Stereotype == PRIM;
        }

        public static bool IsENUM(this Element element)
        {
            return element.Stereotype == ENUM;
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

        private static bool PackageHasStereotype(this Package package, string stereotype)
        {
            return (package.Element != null) && (package.Element.Stereotype == stereotype);
        }

        public static bool IsCCLibrary(this Package package)
        {
            return PackageHasStereotype(package, CCLibrary);
        }

        public static bool IsCDTLibrary(this Package package)
        {
            return PackageHasStereotype(package, CDTLibrary);
        }

        public static bool IsBIELibrary(this Package package)
        {
            return PackageHasStereotype(package, BIELibrary);
        }

        public static bool IsBDTLibrary(this Package package)
        {
            return PackageHasStereotype(package, BDTLibrary);
        }

        public static bool IsPRIMLibrary(this Package package)
        {
            return PackageHasStereotype(package, PRIMLibrary);
        }

        public static bool IsENUMLibrary(this Package package)
        {
            return PackageHasStereotype(package, ENUMLibrary);
        }

        public static bool IsDOCLibrary(this Package package)
        {
            return PackageHasStereotype(package, DOCLibrary);
        }

        public static bool IsBLibrary(this Package package)
        {
            return PackageHasStereotype(package, BLibrary);
        }

        public static bool IsBInformationV(this Package package)
        {
            return PackageHasStereotype(package, BInformationV);
        }
#pragma warning restore 1591

        ///<summary>
        ///</summary>
        ///<typeparam name="T"></typeparam>
        ///<returns>The stereotype string corresponding to the type parameter.</returns>
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
            if (typeof(T) == typeof(IDOCLibrary))
            {
                return DOCLibrary;
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