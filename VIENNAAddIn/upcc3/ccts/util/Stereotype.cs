// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Returns the correct version of a given stereotype string by fixing case. For example: Normalize("ISequIValenttO") == "isEquivalentTo". 
        /// If the given string does not match any defined stereotype, the string is returned unmodified.
        /// </summary>
        /// <param name="stereotype"></param>
        /// <returns></returns>
        public static string Normalize(string stereotype)
        {
            switch (stereotype.ToLower())
            {
                case "abie":
                    return ABIE;
                case "acc":
                    return ACC;
                case "asbie":
                    return ASBIE;
                case "ascc":
                    return ASCC;
                case "basedon":
                    return BasedOn;
                case "bbie":
                    return BBIE;
                case "bcc":
                    return BCC;
                case "bdt":
                    return BDT;
                case "bdtlibrary":
                    return BDTLibrary;
                case "bielibrary":
                    return BIELibrary;
                case "doclibrary":
                    return DOCLibrary;
                case "blibrary":
                    return BLibrary;
                case "cclibrary":
                    return CCLibrary;
                case "binformationv":
                    return BInformationV;
                case "cdt":
                    return CDT;
                case "cdtlibrary":
                    return CDTLibrary;
                case "con":
                    return CON;
                case "enumlibrary":
                    return ENUMLibrary;
                case "isequivalentto":
                    return IsEquivalentTo;
                case "primlibrary":
                    return PRIMLibrary;
                case "sup":
                    return SUP;
                case "prim":
                    return PRIM;
                case "enum":
                    return ENUM;
                default:
                    return stereotype;
            }
        }

        private static readonly List<string> BusinessLibraryStereotypes = new List<string>{BLibrary, PRIMLibrary, ENUMLibrary, CDTLibrary, CCLibrary, BDTLibrary, BIELibrary, DOCLibrary};

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
            return element != null && element.Stereotype == stereotype;
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

        public static bool IsCCLibrary(this Package package)
        {
            return package.HasStereotype(CCLibrary);
        }

        public static bool IsCDTLibrary(this Package package)
        {
            return package.HasStereotype(CDTLibrary);
        }

        public static bool IsBIELibrary(this Package package)
        {
            return package.HasStereotype(BIELibrary);
        }

        public static bool IsBDTLibrary(this Package package)
        {
            return package.HasStereotype(BDTLibrary);
        }

        public static bool IsPRIMLibrary(this Package package)
        {
            return package.HasStereotype(PRIMLibrary);
        }

        public static bool IsENUMLibrary(this Package package)
        {
            return package.HasStereotype(ENUMLibrary);
        }

        public static bool IsDOCLibrary(this Package package)
        {
            return package.HasStereotype(DOCLibrary);
        }

        public static bool IsBLibrary(this Package package)
        {
            return package.HasStereotype(BLibrary);
        }

        public static bool IsBInformationV(this Package package)
        {
            return package.HasStereotype(BInformationV);
        }
#pragma warning restore 1591

        ///<summary>
        ///</summary>
        ///<typeparam name="T"></typeparam>
        ///<returns>The stereotype string corresponding to the type parameter.</returns>
        public static string GetStereotype<T>()
        {
            if (typeof (T) == typeof (IBLibrary))
            {
                return BLibrary;
            }
            if (typeof (T) == typeof (ICCLibrary))
            {
                return CCLibrary;
            }
            if (typeof (T) == typeof (ICDTLibrary))
            {
                return CDTLibrary;
            }
            if (typeof (T) == typeof (IDOCLibrary))
            {
                return DOCLibrary;
            }
            if (typeof (T) == typeof (IBIELibrary))
            {
                return BIELibrary;
            }
            if (typeof (T) == typeof (IBDTLibrary))
            {
                return BDTLibrary;
            }
            if (typeof (T) == typeof (IPRIMLibrary))
            {
                return PRIMLibrary;
            }
            if (typeof (T) == typeof (IENUMLibrary))
            {
                return ENUMLibrary;
            }
            if (typeof (T) == typeof (ICDT))
            {
                return CDT;
            }
            if (typeof (T) == typeof (IACC))
            {
                return ACC;
            }
            if (typeof (T) == typeof (IASCC))
            {
                return ASCC;
            }
            if (typeof (T) == typeof (IBCC))
            {
                return BCC;
            }
            if (typeof (T) == typeof (IBDT))
            {
                return BDT;
            }
            if (typeof (T) == typeof (IABIE))
            {
                return ABIE;
            }
            if (typeof (T) == typeof (IBBIE))
            {
                return BBIE;
            }
            if (typeof (T) == typeof (IASBIE))
            {
                return ASBIE;
            }
            if (typeof (T) == typeof (IPRIM))
            {
                return PRIM;
            }
            if (typeof (T) == typeof (IENUM))
            {
                return ENUM;
            }
            if (typeof (T) == typeof (ISUP))
            {
                return SUP;
            }
            if (typeof (T) == typeof (ICON))
            {
                return CON;
            }
            return String.Empty;
        }

        public static bool IsBusinessLibraryStereotype(string stereotype)
        {
            return BusinessLibraryStereotypes.Contains(stereotype);
        }
    }
}