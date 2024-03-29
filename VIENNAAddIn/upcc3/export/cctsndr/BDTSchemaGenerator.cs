using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using CctsRepository.BdtLibrary;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    ///<summary>
    ///</summary>
    public static class BDTSchemaGenerator
    {
        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="bdts"></param>
        public static void GenerateXSD(GeneratorContext context, IEnumerable<IBdt> bdts)
        {
            var schema = new XmlSchema {TargetNamespace = context.TargetNamespace};
            schema.Namespaces.Add(context.NamespacePrefix, context.TargetNamespace);
            schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            schema.Namespaces.Add("ccts","urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3");
            schema.Version = context.DocLibrary.VersionIdentifier.DefaultTo("1");

            foreach (IBdt bdt in bdts)
            {
                var sups = new List<IBdtSup>(bdt.Sups);
                if (sups.Count == 0)
                {
                    var simpleType = new XmlSchemaSimpleType {Name = NDR.GetXsdTypeNameFromBdt(bdt)};
                    var simpleTypeRestriction = new XmlSchemaSimpleTypeRestriction
                                                {
                                                    BaseTypeName = GetXmlQualifiedName(bdt.Con.BasicType.Name)
                                                };
                    simpleType.Content = simpleTypeRestriction;
                    if (context.Annotate)
                    {
                        simpleType.Annotation = GetTypeAnnotation(bdt);
                    }
                    schema.Items.Add(simpleType);
                }
                else
                {
                    var complexType = new XmlSchemaComplexType
                                      {
                                          Name = NDR.GetXsdTypeNameFromBdt(bdt)
                                      };
                    var simpleContent = new XmlSchemaSimpleContent();
                    var simpleContentExtension = new XmlSchemaSimpleContentExtension
                                                 {
                                                     BaseTypeName = GetXmlQualifiedName(bdt.Con.BasicType.Name)
                                                 };
                    foreach (IBdtSup sup in sups)
                    {
                        var attribute = new XmlSchemaAttribute
                                        {
                                            // Deviation from rule [R ABC1]: Using only attribute name and type as xml attribute name (instead of complete DEN), following the examples given in the specification.
                                            Name = NDR.GetXsdAttributeNameFromSup(sup),
                                            SchemaTypeName = new XmlQualifiedName(GetXSDType(sup.BasicType.Name),
                                                                                  "http://www.w3.org/2001/XMLSchema"),
                                        };
                        if (context.Annotate)
                        {
                            attribute.Annotation = GetAttributeAnnotation(sup);
                        }
                        simpleContentExtension.Attributes.Add(attribute);
                    }

                    simpleContent.Content = simpleContentExtension;
                    complexType.ContentModel = simpleContent;
                    if (context.Annotate)
                    {
                        complexType.Annotation = GetTypeAnnotation(bdt);
                    }
                    schema.Items.Add(complexType);
                }
            }
            context.AddSchema(schema, "BusinessDataType_" + schema.Version + ".xsd");
        }

        private static XmlSchemaAnnotation GetAttributeAnnotation(IBdtSup sup)
        {
            var xml = new XmlDocument();
            // Deviation from rule [R 9C95]: Generating only a subset of the defined annotations and added some additional annotations.
            var annNodes = new List<XmlNode>();
            AddAnnotation(xml, annNodes, "PropertyTermName", sup.Name);
            AddAnnotation(xml, annNodes, "RepresentationTermName", sup.BasicType.Name);
            AddAnnotation(xml, annNodes, "PrimitiveTypeName", sup.BasicType.Name);
            AddAnnotation(xml, annNodes, "DataTypeName", sup.Bdt.Name);
            AddAnnotation(xml, annNodes, "UniqueID", sup.UniqueIdentifier);
            AddAnnotation(xml, annNodes, "VersionID", sup.VersionIdentifier);
            AddAnnotation(xml, annNodes, "DictionaryEntryName", sup.DictionaryEntryName);
            AddAnnotation(xml, annNodes, "Definition", sup.Definition);
            AddAnnotations(xml, annNodes, "BusinessTermName", sup.BusinessTerms);
            AddAnnotation(xml, annNodes, "ModificationAllowedIndicator",
                          sup.ModificationAllowedIndicator.ToString().ToLower());
            AddAnnotation(xml, annNodes, "LanguageCode", sup.LanguageCode);
            AddAnnotation(xml, annNodes, "AcronymCode", "SUP");
            var ann = new XmlSchemaAnnotation();
            ann.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = annNodes.ToArray()});
            return ann;
        }

        private static XmlSchemaAnnotation GetTypeAnnotation(IBdt bdt)
        {
            var xml = new XmlDocument();
            // Deviation from rule [R BFE5]: Generating only a subset of the defined annotations and added some additional annotations.
            var annNodes = new List<XmlNode>();
            AddAnnotation(xml, annNodes, "UniqueID", bdt.UniqueIdentifier);
            AddAnnotation(xml, annNodes, "VersionID", bdt.VersionIdentifier);
            AddAnnotation(xml, annNodes, "DictionaryEntryName", bdt.DictionaryEntryName);
            AddAnnotation(xml, annNodes, "Definition", bdt.Definition);
            AddAnnotations(xml, annNodes, "BusinessTermName", bdt.BusinessTerms);
            AddAnnotation(xml, annNodes, "PropertyTermName", bdt.Name);
            AddAnnotation(xml, annNodes, "LanguageCode", bdt.LanguageCode);
            AddAnnotation(xml, annNodes, "AcronymCode", "BDT");
            var ann = new XmlSchemaAnnotation();
            ann.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = annNodes.ToArray()});
            return ann;
        }

        private static void AddAnnotations(XmlDocument xml, List<XmlNode> annNodes, string name,
                                           IEnumerable<string> values)
        {
            foreach (string item in values)
            {
                AddAnnotation(xml, annNodes, name, item);
            }
        }

        private static void AddAnnotation(XmlDocument xml, List<XmlNode> annNodes, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                XmlElement annotation = xml.CreateElement("ccts", name,
                                                          "urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3");
                annotation.InnerText = value;
                annNodes.Add(annotation);
            }
        }

        private static XmlQualifiedName GetXmlQualifiedName(string basicTypeName)
        {
            return new XmlQualifiedName(GetXSDType(basicTypeName), "http://www.w3.org/2001/XMLSchema");
        }

        private static string GetXSDType(string primitiveTypeName)
        {
            switch (primitiveTypeName.ToLower())
            {
                case "string":
                    return "string";
                case "decimal":
                    return "decimal";
                case "binary":
                    return "base64Binary";
                case "base64binary":
                    return "base64Binary";
                case "token":
                    return "token";
                case "double":
                    return "double";
                case "integer":
                    return "integer";
                case "long":
                    return "long";
                case "datetime":
                    return "dateTime";
                case "date":
                    return "date";
                case "time":
                    return "time";
                case "boolean":
                    return "boolean";
                default:
                    return "string";
            }
        }
    }
}