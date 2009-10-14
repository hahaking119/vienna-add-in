using System;
using System.Collections.Generic;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.import.cctsndr
{
    public class BDTSchemaImporter
    {
        private readonly XmlSchema schema;
        private readonly IBDTLibrary bdtLibrary;
        private readonly IPRIMLibrary primLibrary;

        public BDTSchemaImporter(IImporterContext context)
        {
            schema = context.BDTSchema;
            bdtLibrary = context.BDTLibrary;
            primLibrary = context.PRIMLibrary;
        }

        public void ImportXSD()
        {
            foreach (XmlSchemaObject currentElement in schema.Items)
            {
                if (currentElement is XmlSchemaComplexType)
                {
                    ImportComplexType((XmlSchemaComplexType) currentElement);
                }
                else if (currentElement is XmlSchemaSimpleType)
                {
                    ImportSimpleType((XmlSchemaSimpleType) currentElement);
                }
            }
        }

        private void ImportSimpleType(XmlSchemaSimpleType simpleType)
        {
            // TODO: implement the above import schema for XSD Simple Types (Cast problem not solved yet.)
            var simpleContent = (XmlSchemaSimpleTypeRestriction) simpleType.Content;
            var conPrimName = ConvertXsdTypeNameToBasicTypeName(simpleContent.BaseTypeName.Name);
            CreateBDT(simpleType, conPrimName, new List<SUPSpec>());
        }

        private void ImportComplexType(XmlSchemaComplexType complexType)
        {
            var simpleContent = (XmlSchemaSimpleContentExtension) complexType.ContentModel.Content;
            String conPrimName = ConvertXsdTypeNameToBasicTypeName(simpleContent.BaseTypeName.Name);
            var sups = SpecifySUPs(simpleContent.Attributes, conPrimName);
            CreateBDT(complexType, conPrimName, sups);
        }

        private CONSpec SpecifyCON(string primName)
        {
            return new CONSpec
                   {
                       BasicType = FindPRIM(primName)
                   };
        }

        private IEnumerable<SUPSpec> SpecifySUPs(XmlSchemaObjectCollection xsdAttributes, string conPrimName)
        {
            foreach (XmlSchemaAttribute attribute in xsdAttributes)
            {
                yield return SpecifySUP(attribute, conPrimName,
                                        ConvertXsdTypeNameToBasicTypeName(attribute.SchemaTypeName.ToString().Replace("http://www.w3.org/2001/XMLSchema", string.Empty)));
            }
        }

        private SUPSpec SpecifySUP(XmlSchemaAttribute attribute, string conPrimName, string primName)
        {
            return new SUPSpec
                   {
                       Name = attribute.Name.Replace(conPrimName, string.Empty),
                       BasicType = FindPRIM(primName)
                   };
        }

        private IPRIM FindPRIM(string primName)
        {
            var prim = primLibrary.ElementByName(primName);
            if (prim == null)
            {
                throw new Exception(string.Format("PRIM not found: {0}", primName));
            }
            return prim;
        }

        private void CreateBDT(XmlSchemaType xsdType, string conPrimName, IEnumerable<SUPSpec> sups)
        {
            var bdtSpec = new BDTSpec
                          {
                              Name = NDR.GetBdtNameFromXsdType(xsdType),
                              CON = SpecifyCON(conPrimName),
                              SUPs = new List<SUPSpec>(sups),
                          };
            bdtLibrary.CreateElement(bdtSpec);
        }

        private static string ConvertXsdTypeNameToBasicTypeName(string xsdTypeName)
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
                    return "String";
            }
        }
    }
}