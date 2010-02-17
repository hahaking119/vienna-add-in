using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.export.mapping
{
    public class SubsetExporter
    {
        private readonly XmlSchemaSet xmlSchemaSet;

        public SubsetExporter(string schemaFileComplete)
        {
            xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(schemaFileComplete), null));
            xmlSchemaSet.Compile();
        }

        public static void ExportSubset(IDocLibrary docLibrary, string schemaFileComplete, string schemaDirectorySubset)
        {
            SubsetExporter exporter = new SubsetExporter(schemaFileComplete);

            exporter.ExecuteXmlSchemaSubsetting(docLibrary);

            exporter.WriteXmlSchemaSubset(schemaDirectorySubset);
        }

        public void ExecuteXmlSchemaSubsetting(IDocLibrary docLibrary)
        {
            UpccModelXsdTypes remainingXsdTypes = new UpccModelXsdTypes(docLibrary);

            foreach (XmlSchema xmlSchema in xmlSchemaSet.Schemas())
            {
                XmlSchemaObjectTable xmlSchemaObjectTable = xmlSchema.SchemaTypes;

                foreach (XmlSchemaObject type in CopyValues(xmlSchemaObjectTable))
                {
                    if (type is XmlSchemaComplexType)
                    {
                        string complexTypeName = ((XmlSchemaComplexType)type).QualifiedName.Name;

                        if (remainingXsdTypes.ContainsXsdType(complexTypeName))
                        {
                            RemoveElementsAndAttributesFromComplexType((XmlSchemaComplexType)type, remainingXsdTypes);
                        }
                        else
                        {
                            xmlSchema.Items.Remove(type);                            
                        }
                    }
                    if (type is XmlSchemaSimpleType)
                    {
                        string simpleTypeName = ((XmlSchemaSimpleType)type).QualifiedName.Name;

                        if (!remainingXsdTypes.ContainsXsdType(simpleTypeName))
                        {                            
                            xmlSchema.Items.Remove(type);
                        }
                    }
                }
                
                RemoveGlobalElementsHavingAnInvalidXsdType(xmlSchema, remainingXsdTypes);
                RemoveGlobalAttributesHavingAnInvalidXsdType(xmlSchema, remainingXsdTypes);

                // NOTE: after we removed all global elements and all global attributes 
                // we still have the problem that there are complex types referring to those 
                // elements and/or attributes. Therefore, it is necessary to adapt the complex
                // types accordingly. 
            }
        }

        private void RemoveGlobalAttributesHavingAnInvalidXsdType(XmlSchema xmlSchema, UpccModelXsdTypes remainingXsdTypes)
        {           
            foreach (XmlSchemaObject element in CopyValues(xmlSchema.Attributes))
            {
                if (element is XmlSchemaAttribute)
                {
                    // NOTE: the problem was that we must use "AttributeSchemaType" instead of "SchemaType"
                    string schemaTypeNameInUse = ((XmlSchemaAttribute)element).AttributeSchemaType.QualifiedName.Name;

                    if (!remainingXsdTypes.ContainsXsdType(schemaTypeNameInUse))
                    {
                        xmlSchema.Items.Remove(element);                            
                    }
                }
            }
        }

        private void RemoveGlobalElementsHavingAnInvalidXsdType(XmlSchema xmlSchema, UpccModelXsdTypes remainingXsdTypes)
        {
            foreach (XmlSchemaObject element in CopyValues(xmlSchema.Elements))
            {
                if (element is XmlSchemaElement)
                {
                    // NOTE: the problem was that we must use "ElementSchemaType" instead of "SchemaType"
                    string schemaTypeNameInUse = ((XmlSchemaElement)element).ElementSchemaType.QualifiedName.Name;

                    if (!remainingXsdTypes.ContainsXsdType(schemaTypeNameInUse))
                    {
                        xmlSchema.Items.Remove(element);
                    }
                }
            }
        }

        private static void WriteXmlSchema(XmlSchema xmlSchema, string xmlSchemaFile)
        {
            var xmlWriterSettings = new XmlWriterSettings
                                        {
                                            Indent = true,
                                            Encoding = Encoding.UTF8,
                                        };

            using (var xmlWriter = XmlWriter.Create(xmlSchemaFile, xmlWriterSettings))
            {
                if (xmlWriter != null)
                {
                    xmlSchema.Write(xmlWriter);
                    xmlWriter.Close();
                }
            }
        }


        private IEnumerable<XmlSchemaObject> CopyValues(XmlSchemaObjectTable objectTable)
        {
            XmlSchemaObject[] objectTableCopy = new XmlSchemaObject[objectTable.Count];

            objectTable.Values.CopyTo(objectTableCopy, 0);

            return objectTableCopy;
        }

        private IEnumerable<XmlSchemaObject> CopyValues(XmlSchemaObjectCollection objectCollection)
        {
            XmlSchemaObject[] objectCollectionCopy = new XmlSchemaObject[objectCollection.Count];

            objectCollection.CopyTo(objectCollectionCopy, 0);

            return objectCollectionCopy;
        }


        private void RemoveElementsAndAttributesFromComplexType(XmlSchemaComplexType complexType, UpccModelXsdTypes remainingXsdTypes)
        {
            if (complexType.Particle is XmlSchemaGroupBase)
            {
                RemoveElementsFromXsdGroup((XmlSchemaGroupBase)complexType.Particle, complexType.QualifiedName.Name, remainingXsdTypes);
            }

            if (complexType.ContentModel is XmlSchemaSimpleContent)
            {
                XmlSchemaSimpleContent contentModel = (XmlSchemaSimpleContent)complexType.ContentModel;

                if (contentModel.Content is XmlSchemaSimpleContentExtension)
                {
                    RemoveAttributesFromComplexType(((XmlSchemaSimpleContentExtension)contentModel.Content).Attributes, complexType.QualifiedName.Name, remainingXsdTypes);                    
                }
                else if (contentModel.Content is XmlSchemaSimpleContentRestriction)
                {
                    RemoveAttributesFromComplexType(((XmlSchemaSimpleContentRestriction)contentModel.Content).Attributes, complexType.QualifiedName.Name, remainingXsdTypes);                    
                }
            }

            RemoveAttributesFromComplexType(complexType.Attributes, complexType.QualifiedName.Name, remainingXsdTypes);
        }

        private void RemoveElementsFromXsdGroup(XmlSchemaGroupBase xsdGroup, string xsdTypeName, UpccModelXsdTypes remainingXsdTypes)
        {
            foreach (XmlSchemaObject item in CopyValues(xsdGroup.Items))
            {
                if (item is XmlSchemaElement)
                {
                    string childName = ((XmlSchemaElement) item).QualifiedName.Name;
                    
                    if (!(remainingXsdTypes.XsdTypeContainsChild(xsdTypeName, childName)))
                    {
                        xsdGroup.Items.Remove(item);
                    }
                }
                else if (item is XmlSchemaGroupBase)
                {
                    RemoveElementsFromXsdGroup((XmlSchemaGroupBase) item, xsdTypeName, remainingXsdTypes);
                }
            }
        }

        private void RemoveAttributesFromComplexType(XmlSchemaObjectCollection attributes, string xsdTypeName, UpccModelXsdTypes remainingXsdTypes)
        {
            foreach (XmlSchemaAttribute attribute in CopyValues(attributes))
            {
                string attributeName = attribute.QualifiedName.Name;

                if (!(remainingXsdTypes.XsdTypeContainsChild(xsdTypeName, attributeName)))
                {
                    attributes.Remove(attribute);                    
                }                
            }
        }

        public void WriteXmlSchemaSubset(string schemaDirectorySubset)
        {
            if (!(Directory.Exists(schemaDirectorySubset)))
            {
                Directory.CreateDirectory(schemaDirectorySubset);
            }
            
            foreach (XmlSchema xmlSchema in xmlSchemaSet.Schemas())
            {
                int lastIndexOfSlash = xmlSchema.SourceUri.LastIndexOf("/");
                string fileName = xmlSchema.SourceUri.Substring(lastIndexOfSlash + 1);
                WriteXmlSchema(xmlSchema, schemaDirectorySubset + fileName);
            }  
        }
    }
}
