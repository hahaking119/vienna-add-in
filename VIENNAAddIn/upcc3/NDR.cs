using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using UPCCRepositoryInterface;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddIn.upcc3.import.util;

namespace VIENNAAddIn.upcc3
{
    public static class NDR
    {
        public const string CCTSNamespace = "urn:un:unece:uncefact:documentation:common:3:standard:CoreComponentsTechnicalSpecification:3";
        private const string Identification = "Identification";
        private const string Identifier = "Identifier";
        private const string Indication = "Indication";
        private const string Indicator = "Indicator";
        private const string Text = "Text";

        public static string GenerateBCCName(IBCC bcc)
        {
            return GenerateBCCOrBBIEName(bcc.Name, bcc.Type.Name, bcc.DictionaryEntryName);
        }

        public static string GetXsdElementNameFromBbie(IBBIE bbie)
        {
            return GenerateBCCOrBBIEName(bbie.Name, bbie.Type.Name, bbie.DictionaryEntryName);
        }

        /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
        private static string GenerateBCCOrBBIEName(string propertyTerm, string representationTerm, string dictionaryEntryName)
        {
            if (!String.IsNullOrEmpty(dictionaryEntryName))
            {
                string[] parts = dictionaryEntryName.Replace(" ", String.Empty).Replace("-", String.Empty).Split('.');
                if (parts.Length != 3)
                {
                    throw new ArgumentException(
                        "Expected DictionaryEntryName <ObjectClassTerm. PropertyTerm. RepresentationTerm>, but is " +
                        dictionaryEntryName);
                }
                propertyTerm = parts[1];
                representationTerm = parts[2];
            }
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

        public static string GetXsdElementNameFromAsbie(IASBIE asbie)
        {
            return asbie.Name + asbie.AssociatedElement.Name;
        }

        public static string GetAsbieNameFromXsdElement(Element element, string associatedElementName)
        {
            return element.Ref.Name.Minus(associatedElementName);
        }

        /// <summary>
        /// [R A7B8]
        /// The name of a BDT that uses a primitive to define its content component BVD MUST be
        /// - the BDT ccts:DataTypeQualifier(s) if any, plus
        /// - the ccts:DataTypeTerm, plus
        /// - the primitive type name,
        /// - followed by the word �Type�
        /// - with the separators removed and approved abbreviations and acronyms applied.
        /// Deviation: Separators ('_') are not removed.
        ///
        /// [R 90FB]
        /// The name of a BDT that includes one or more Supplementary Components MUST be:
        /// - The BDT ccts:DataTypeQualifier(s) if any, plus
        /// - The ccts:DataTypeTerm, plus
        /// - The suffix of the Content Component Business Value Domain where the suffix is 
        ///   the primitive type name, the code list token, the series of code list tokens, 
        ///   or the identifier scheme token.
        /// plus
        /// - The ccts:DictionaryEntryName for each Supplementary Component present following the order
        ///   defined in the Data Type Catalogue, plus
        /// - The suffix that represents the Supplementary Component BVD where the suffix is the primitive 
        ///   type name, the code list token, the series of code list tokens, or the identifier scheme token, plus
        /// - The word �Type�.
        /// - With all separators removed and approved abbreviations and acronyms applied.
        /// Deviation: Ignoring the SUPs, which means that we name complex types in the same way as simple types.
        /// Deviation: Separators ('_') are not removed.
        /// </summary>
        /// <param name="bdt"></param>
        /// <returns></returns>
        public static string GetXsdTypeNameFromBdt(IBDT bdt)
        {
            return bdt.Name + bdt.CON.BasicType.Name + "Type";
        }

        public static string ConvertXsdTypeNameToBasicTypeName(string xsdTypeName)
        {
            switch (xsdTypeName.ToLower())
            {
                case "string":
                    return "String";
                case "decimal":
                    return "Decimal";
                case "base64binary":
                    return "Base64Binary";
                case "token":
                    return "Token";
                case "double":
                    return "Double";
                case "integer":
                    return "Integer";
                case "long":
                    return "Long";
                case "datetime":
                    return "DateTime";
                case "date":
                    return "Date";
                case "time":
                    return "Time";
                case "boolean":
                    return "Boolean";
                default:
                    throw new Exception("Undefined XSD datatype name: " + xsdTypeName);
            }
        }

        public static bool IsXsdDataTypeName(XmlQualifiedName typeName)
        {
            return typeName.Namespace == "http://www.w3.org/2001/XMLSchema";
        }
    }
}