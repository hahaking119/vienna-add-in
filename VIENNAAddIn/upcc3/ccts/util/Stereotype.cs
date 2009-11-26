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
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.bLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;

namespace VIENNAAddIn.upcc3.ccts.util
{
    ///<summary>
    /// Definition of stereotype strings for UPCC elements.
    ///</summary>
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
        public const string DOCLibrary = "DOCLibrary";
        public const string bLibrary = "bLibrary";
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
        public const string CodelistEntry = "codelistEntry";

        /// <summary>
        /// Returns the correct version of a given stereotype string by fixing case.
        /// 
        /// For example: Normalize("ISequIValenttO") == "isEquivalentTo". 
        /// 
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
                    return bLibrary;
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
                case "codelistentry":
                    return CodelistEntry;
                default:
                    return stereotype;
            }
        }

        private static readonly List<string> BusinessLibraryStereotypes = new List<string>
                                                                          {
                                                                              bLibrary,
                                                                              PRIMLibrary,
                                                                              ENUMLibrary,
                                                                              CDTLibrary,
                                                                              CCLibrary,
                                                                              BDTLibrary,
                                                                              BIELibrary,
                                                                              DOCLibrary
                                                                          };

        /// <returns>True if the given stereotype is one of the stereotypes for business libraries, false otherwise.</returns>
        public static bool IsBusinessLibraryStereotype(string stereotype)
        {
            return BusinessLibraryStereotypes.Contains(stereotype);
        }

        /// <returns>
        /// The stereotype string corresponding to the type parameter, for example "bLibrary" for IBLibrary, 
        /// or an empty string if the type does not map to a stereotype.
        /// </returns>
        public static string GetStereotype<T>()
        {
            if (typeof (T) == typeof (IBLibrary))
            {
                return bLibrary;
            }
            if (typeof (T) == typeof (ICcLibrary))
            {
                return CCLibrary;
            }
            if (typeof (T) == typeof (ICdtLibrary))
            {
                return CDTLibrary;
            }
            if (typeof (T) == typeof (IDOCLibrary))
            {
                return DOCLibrary;
            }
            if (typeof (T) == typeof (IBieLibrary))
            {
                return BIELibrary;
            }
            if (typeof (T) == typeof (IBdtLibrary))
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
            if (typeof (T) == typeof (ICdt))
            {
                return CDT;
            }
            if (typeof (T) == typeof (IAcc))
            {
                return ACC;
            }
            if (typeof (T) == typeof (IAscc))
            {
                return ASCC;
            }
            if (typeof (T) == typeof (IBcc))
            {
                return BCC;
            }
            if (typeof (T) == typeof (IBdt))
            {
                return BDT;
            }
            if (typeof (T) == typeof (IAbie))
            {
                return ABIE;
            }
            if (typeof (T) == typeof (IBbie))
            {
                return BBIE;
            }
            if (typeof (T) == typeof (IAsbie))
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
            if (typeof (T) == typeof (ICdtSup))
            {
                return SUP;
            }
            if (typeof (T) == typeof (ICdtCon))
            {
                return CON;
            }
            if (typeof (T) == typeof (IBdtSup))
            {
                return SUP;
            }
            if (typeof (T) == typeof (IBdtCon))
            {
                return CON;
            }
            if (typeof (T) == typeof (ICodelistEntry))
            {
                return CodelistEntry;
            }
            return String.Empty;
        }
    }
}