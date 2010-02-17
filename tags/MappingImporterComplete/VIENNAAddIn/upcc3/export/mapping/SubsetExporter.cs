using System;
using System.Collections;
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

        public static void ExportSubset(IDocLibrary docLibrary, string schemaFileComplete, string schemaFileSubset)
        {
            SubsetExporter exporter = new SubsetExporter(schemaFileComplete);

            exporter.ExecuteXmlSchemaSubsetting(docLibrary);

            exporter.WriteXmlSchemaSubset(schemaFileSubset);
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
                    // TODO: type is XmlSchemaSimpleType
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
                RemoveElementsAndAttributesFromXsdGroup((XmlSchemaGroupBase)complexType.Particle, complexType.QualifiedName.Name, remainingXsdTypes);
            }

            //if (complexType.ContentModel is XmlSchemaSimpleContent)
            //{
            //    XmlSchemaSimpleContent contentModel = (XmlSchemaSimpleContent) complexType.ContentModel;

            //    if (contentModel.Content is XmlSchemaSimpleContentExtension)
            //    {
            //        foreach (XmlSchemaObject attribute in ((XmlSchemaSimpleContentExtension) contentModel.Content).Attributes)
            //        {
            //            myChildren.Add(attribute);
            //        }
            //    }
            //    else if (contentModel.Content is XmlSchemaSimpleContentRestriction)
            //    {
            //        foreach (
            //            XmlSchemaObject attribute in
            //                ((XmlSchemaSimpleContentRestriction) contentModel.Content).Attributes)
            //        {
            //            myChildren.Add(attribute);
            //        }
            //    }
            //}

            //foreach (XmlSchemaAttribute attribute in complexType.Attributes)
            //{
            //    myChildren.Add(attribute);
            //}
        }

        private void RemoveElementsAndAttributesFromXsdGroup(XmlSchemaGroupBase xsdGroup, string xsdTypeName, UpccModelXsdTypes remainingXsdTypes)
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
                    RemoveElementsAndAttributesFromXsdGroup((XmlSchemaGroupBase) item, xsdTypeName, remainingXsdTypes);
                }
            }
        }

        public void WriteXmlSchemaSubset(string schemaFileSubset)
        {
            foreach (XmlSchema xmlSchema in xmlSchemaSet.Schemas())
            {
                WriteXmlSchema(xmlSchema, schemaFileSubset);
            }  
        }
    }
}
