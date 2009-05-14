using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;

namespace VIENNAAddIn.upcc3.XSDImporter.ccts
{
    public class BDTSchemaImporter
    {
        public static void ImportXSD(ImporterContext context)
        {
            ICCRepository repo = context.Repository;
            List<SchemaInfo> schemas = context.Schemas;
            foreach (SchemaInfo currentSchemaInfo in schemas)
            {
                if(currentSchemaInfo.FileName.StartsWith("BusinessDataType"))
                {
                    XmlSchema currentSchema = currentSchemaInfo.Schema;
                    foreach (var currentElement in currentSchema.Items)
                    {
                        if (currentElement.GetType().Name == "XmlSchemaComplexType")
                        {
                            XmlSchemaComplexType complexType = (XmlSchemaComplexType) currentElement;
                            XmlSchemaSimpleContent simpleContent = (XmlSchemaSimpleContent) complexType.ContentModel;
                            XmlSchemaSimpleContentExtension simpleContentExtension =
                                (XmlSchemaSimpleContentExtension) simpleContent.Content;
                            String baseTypeName = ConvertXSDType(simpleContentExtension.BaseTypeName.Name);
                            
                            BDTLibrary bdtl = repo.Libraries<BDTLibrary>().ElementAt(0);
                            PRIMLibrary primlib = repo.Libraries<PRIMLibrary>().ElementAt(0);

                            
                            CONSpec conSpec = new CONSpec();
                            conSpec.Name = baseTypeName;
                            
                            BDTSpec bdtSpec = new BDTSpec();
                            Debug.WriteLine("Found ComplexType: " + complexType.Name.Replace("Type", "").Replace(baseTypeName, ""));
                            bdtSpec.Name = complexType.Name.Replace("Type", "").Replace(ConvertXSDType(baseTypeName), "");
                            bdtSpec.CON = conSpec;
                            Console.WriteLine(complexType.Attributes.Count);
                            foreach (XmlSchemaAttribute attribute in simpleContentExtension.Attributes)
                            {
                                Console.WriteLine("Found Attribute: " + attribute.Name.Replace(baseTypeName, ""));
                                SUPSpec supSpec = new SUPSpec();
                                supSpec.Name = attribute.Name.Replace(baseTypeName, "");

                                Console.WriteLine("Schema Type Name: " + attribute.SchemaTypeName);
                                foreach (var primitiveType in primlib.Elements)
                                {
                                    if (primitiveType.Name == ConvertXSDType(attribute.SchemaTypeName.ToString().Replace("http://www.w3.org/2001/XMLSchema", "")))
                                    {
                                        Console.WriteLine("Found PrimitiveType: "+primitiveType.Name);
                                        supSpec.BasicType = primitiveType;
                                    }
                                }
                                bdtSpec.SUPs.Add(supSpec);
                            }
                            bdtl.CreateElement(bdtSpec);
                            
                            
                        }
                        //TODO: implement the above import schema for XSD Simple Types (Cast problem not solved yet.)
                        if(currentElement.GetType().Name == "XMLSchemaSimpleType")
                        {
                            XmlSchemaSimpleType simpleType = (XmlSchemaSimpleType) currentElement;
                            Console.WriteLine("Found SimpleType: "+simpleType.Name);
                            //XmlSchemaSimpleContentExtension simpleContentExtension = (XmlSchemaSimpleContentExtension) simpleType.Content;


                        }
                    }
                }
            }
        }
        private static string ConvertXSDType(string XSDTypeName)
        {
            switch (XSDTypeName.ToLower())
            {
                case "string":
                    return "String";
                case "decimal":
                    return "Decimal";
                case "base64Binary":
                    return "Base64binary";
                case "token":
                    return "Token";
                case "double":
                    return "Double";
                case "integer":
                    return "Integer";
                case "long":
                    return "Long";
                case "dateTime":
                    return "Datetime";
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